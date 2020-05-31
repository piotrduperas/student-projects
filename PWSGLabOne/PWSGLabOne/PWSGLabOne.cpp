#include "pch.h"
#include "framework.h"
#include "PWSGLabOne.h"
#include <Uxtheme.h>
#include <list>

#define MAX_LOADSTRING 100
#define COLORS_COUNT 6
#define STATE_INACTIVE 1
#define STATE_INITIALISATION 2
#define STATE_FIREWORKS 3
#define STATE_ACTIVE 4

struct gridEl {
    int x, tx;
    int y, ty;
    int s, ts;
    bool hover;
    int gridX, gridY;
    bool active;
    bool empty;
    int color;
};

struct gwin {
    HWND w;
    gridEl* state;
};

struct particle {
    int color;
    int x, y;
    int dx, dy;
};

HINSTANCE hInst;
HWND mainWindow, particleWindow;
HBRUSH brushes[COLORS_COUNT + 1];
HBRUSH brushesCrossed[COLORS_COUNT + 1];
int gridsize = 1;
bool debugIsOn = false;
POINT chosen = { -1, -1 };
gwin* wingrid = nullptr;
int wingridsize = 0;
int gameState = STATE_INACTIVE;
std::list<particle> particleList;

ATOM MyRegisterClass(HINSTANCE hInstance);
ATOM MyRegisterClassGrid(HINSTANCE hInstance);
ATOM MyRegisterClassParticles(HINSTANCE hInstance);
BOOL InitInstance(HINSTANCE, int);
LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);
LRESULT CALLBACK WndProcGrid(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
LRESULT CALLBACK WndProcParticles(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
INT_PTR CALLBACK About(HWND, UINT, WPARAM, LPARAM);
void newBoard();

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
    _In_opt_ HINSTANCE hPrevInstance, _In_ LPWSTR lpCmdLine,
    _In_ int nCmdShow) {
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    MyRegisterClass(hInstance);
    MyRegisterClassGrid(hInstance);
    MyRegisterClassParticles(hInstance);

    if (!InitInstance(hInstance, nCmdShow)) {
        return FALSE;
    }

    HACCEL hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_PWSGLABONE));
    DWORD dwFlags = (STAP_ALLOW_NONCLIENT |
        STAP_ALLOW_CONTROLS | STAP_ALLOW_WEBCONTENT);
    //SetThemeAppProperties(dwFlags);
    SetWindowTheme(mainWindow, L" ", L" ");
    MSG msg;
    while (GetMessage(&msg, nullptr, 0, 0)) {
        if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg)) {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }

    return (int)msg.wParam;
}

struct grid {
    int elems;
    int pxsize;
};

grid getGridSize() {
    grid g = { 0 };
    switch (gridsize) {
    case 1:
        g.elems = 8;
        g.pxsize = 80;
        break;
    case 2:
        g.elems = 10;
        g.pxsize = 70;
        break;
    case 3:
        g.elems = 12;
        g.pxsize = 60;
        break;
    }
    return g;
}

ATOM MyRegisterClass(HINSTANCE hInstance) {
    WNDCLASSEXW wcex = { 0 };
    wcex.cbSize = sizeof(WNDCLASSEX);
    wcex.lpfnWndProc = WndProc;
    wcex.hInstance = hInstance;
    wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_PWSGLABONE));
    wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    wcex.lpszMenuName = MAKEINTRESOURCEW(IDC_PWSGLABONE);
    wcex.lpszClassName = L"main";
    wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

    return RegisterClassExW(&wcex);
}

ATOM MyRegisterClassGrid(HINSTANCE hInstance) {
    WNDCLASSEXW wcex = { 0 };
    wcex.cbSize = sizeof(WNDCLASSEX);
    wcex.lpfnWndProc = WndProcGrid;
    wcex.hInstance = hInstance;
    wcex.hbrBackground = (HBRUSH)(CreateSolidBrush(RGB(10, 10, 10)));
    wcex.lpszClassName = L"grid";

    return RegisterClassExW(&wcex);
}

ATOM MyRegisterClassParticles(HINSTANCE hInstance) {
    WNDCLASSEXW wcex = { 0 };
    wcex.cbSize = sizeof(WNDCLASSEX);
    wcex.lpfnWndProc = WndProcParticles;
    wcex.hInstance = hInstance;
    wcex.hbrBackground = (HBRUSH)(CreateSolidBrush(RGB(255, 255, 255)));
    wcex.lpszClassName = L"particles";

    return RegisterClassExW(&wcex);
}

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow) {
    hInst = hInstance;

    RECT rt = { 0, 0, 290, 290 };
    AdjustWindowRect(&rt, WS_OVERLAPPEDWINDOW, TRUE);
    int w = rt.right - rt.left;
    int h = rt.bottom - rt.top;

    mainWindow = CreateWindowExW(WS_EX_COMPOSITED, L"main", TEXT("BeWindowed 2020 aka. BeJeweled in WinApi"),
        (WS_OVERLAPPEDWINDOW ^ WS_THICKFRAME) | WS_CLIPCHILDREN, (GetSystemMetrics(SM_CXSCREEN) - w) / 2,
        (GetSystemMetrics(SM_CYSCREEN) - h) / 2, w, h, nullptr, nullptr, hInstance, nullptr);

    if (!mainWindow) {
        return FALSE;
    }

    particleWindow = CreateWindowExW(WS_EX_LAYERED, L"particles", nullptr, WS_POPUP, 0, 0,
        GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN), mainWindow, nullptr, hInstance, nullptr);
    SetLayeredWindowAttributes(particleWindow, RGB(255, 255, 255), 0, LWA_COLORKEY);

    CheckMenuItem(GetMenu(mainWindow), ID_BOARDSIZE_1, MF_CHECKED);

    ShowWindow(mainWindow, nCmdShow);
    UpdateWindow(mainWindow);
    ShowWindow(particleWindow, nCmdShow);
    UpdateWindow(particleWindow);

    COLORREF colors[] = { RGB(255, 0, 0), RGB(255, 255, 0), RGB(255, 0, 255), RGB(0, 255, 0), RGB(0, 0, 255), RGB(0, 255, 255) };

    brushes[0] = (HBRUSH)CreateSolidBrush(RGB(30, 30, 30));
    for (int i = 1; i < COLORS_COUNT + 1; i++) {
        brushes[i] = (HBRUSH)CreateSolidBrush(colors[i - 1]);
        brushesCrossed[i] = (HBRUSH)CreateHatchBrush(HS_CROSS, colors[i - 1]);
    }

    newBoard();
    return TRUE;
}

gwin* getGridEl(int x, int y) {
    return &wingrid[y * getGridSize().elems + x];
}

void redraw(HWND hWnd) {
    InvalidateRect(hWnd, NULL, TRUE);
    UpdateWindow(hWnd);
}

void newGame() {
    gameState = STATE_INITIALISATION;

    grid g = getGridSize();

    for (int y = 0; y < g.elems; y++) {
        for (int x = 0; x < g.elems; x++) {
            getGridEl(x, y)->state->active = false;
            getGridEl(x, y)->state->color = 1 + rand() % COLORS_COUNT;
            SetTimer(getGridEl(x, y)->w, 9, 50 * (y * g.elems + x), NULL);
            redraw(getGridEl(x, y)->w);
        }
    }
    SetTimer(mainWindow, 6, 50 * (g.elems * g.elems), NULL);
    chosen = { -1, -1 };
}

void newBoard() {
    gameState = STATE_INACTIVE;
    InvalidateRect(mainWindow, NULL, TRUE);
    UpdateWindow(mainWindow);
    grid g = getGridSize();

    RECT rt = { 0, 0, g.elems * (g.pxsize + 10), g.elems * (g.pxsize + 10) };
    AdjustWindowRect(&rt, WS_OVERLAPPEDWINDOW, TRUE);
    int w = rt.right - rt.left;
    int h = rt.bottom - rt.top;

    MoveWindow(mainWindow, (GetSystemMetrics(SM_CXSCREEN) - w) / 2, (GetSystemMetrics(SM_CYSCREEN) - h) / 2, w, h, TRUE);

    if (wingrid) {
        for (int i = 0; i < wingridsize; i++) {
            DestroyWindow(wingrid[i].w);
        }
        delete[] wingrid;
    }

    wingridsize = g.elems * g.elems;
    wingrid = new gwin[wingridsize];

    for (int y = 0; y < g.elems; y++) {
        for (int x = 0; x < g.elems; x++) {
            gridEl* e = new gridEl;
            e->x = e->tx = x * (g.pxsize + 10) + 5;
            e->y = e->ty = y * (g.pxsize + 10) + 5;
            e->s = e->ts = g.pxsize;
            e->hover = false;
            e->active = false;
            e->empty = false;
            e->gridX = x;
            e->gridY = y;
            getGridEl(x, y)->state = e;
            getGridEl(x, y)->w = CreateWindowExW(WS_EX_COMPOSITED, L"grid", nullptr, WS_CHILD,
                e->x, e->y, e->s, e->s, mainWindow, nullptr, hInst, e);

            ShowWindow(getGridEl(x, y)->w, SW_SHOW);
            UpdateWindow(getGridEl(x, y)->w);
        }
    }
    UpdateWindow(mainWindow);
}

void selectBoardMenu(HMENU hMenu, int action) {
    CheckMenuItem(hMenu, ID_BOARDSIZE_1, MF_UNCHECKED);
    CheckMenuItem(hMenu, ID_BOARDSIZE_2, MF_UNCHECKED);
    CheckMenuItem(hMenu, ID_BOARDSIZE_3, MF_UNCHECKED);
    CheckMenuItem(hMenu, action, MF_CHECKED);
}

void trackMouse(HWND hWnd) {
    TRACKMOUSEEVENT t = { 0 };
    t.cbSize = sizeof(TRACKMOUSEEVENT);
    t.dwFlags = TME_HOVER | TME_LEAVE;
    t.hwndTrack = hWnd;
    t.dwHoverTime = 10;
    TrackMouseEvent(&t);
}

gridEl* getGridData(HWND hWnd) {
    LONG_PTR ptr = GetWindowLongPtr(hWnd, GWLP_USERDATA);
    return reinterpret_cast<gridEl*>(ptr);
}

void emptyAndFireworks(int x, int y) {
    getGridEl(x, y)->state->empty = true;
    redraw(getGridEl(x, y)->w);
    RECT rc;
    GetWindowRect(getGridEl(x, y)->w, &rc);

    for (int i = 0; i < 100; i++) {
        particle p;
        p.color = getGridEl(x, y)->state->color;
        p.x = rc.left + rand() % (rc.right - rc.left);
        p.y = rc.top + rand() % (rc.bottom - rc.top);
        p.dx = (rand() % 2 ? -1 : 1) * (rand() % 22 + 4);
        p.dy = (rand() % 2 ? -1 : 1) * (rand() % 22 + 4);
        particleList.push_back(p);
    }
}

bool checkSeries() {
    std::list<POINT> toRemove;
    grid g = getGridSize();
    for (int y = 0; y < g.elems; y++) {
        for (int x = 2; x < g.elems; x++) {
            int c = getGridEl(x, y)->state->color;
            if (getGridEl(x - 2, y)->state->color == c && getGridEl(x - 1, y)->state->color == c) {
                int s = x - 2;
                while (s < g.elems && getGridEl(s, y)->state->color == c) {
                    toRemove.push_back({ s, y });
                    s++;
                }
            }
            c = getGridEl(y, x)->state->color;
            if (getGridEl(y, x - 2)->state->color == c && getGridEl(y, x - 1)->state->color == c) {
                int s = x - 2;
                while (s < g.elems&& getGridEl(y, s)->state->color == c) {
                    toRemove.push_back({ y, s });
                    s++;
                }
            }
        }
    }
    for (auto& it : toRemove) {
        emptyAndFireworks(it.x, it.y);
    }
    return toRemove.size();
}

bool fallDown() {
    grid g = getGridSize();
    bool anything = false;
    for (int y = g.elems - 1; y > 0; y--) {
        for (int x = 0; x < g.elems; x++) {
            if (getGridEl(x, y)->state->empty) {
                anything = true;
                if (!getGridEl(x, y - 1)->state->empty) {
                    getGridEl(x, y)->state->empty = false;
                    getGridEl(x, y - 1)->state->empty = true;
                    getGridEl(x, y)->state->color = getGridEl(x, y - 1)->state->color;
                    redraw(getGridEl(x, y)->w);
                    redraw(getGridEl(x, y - 1)->w);
                }
            }
        }
    }
    for (int x = 0; x < g.elems; x++) {
        if (getGridEl(x, 0)->state->empty) {
            getGridEl(x, 0)->state->empty = false;
            getGridEl(x, 0)->state->color = 1 + rand() % COLORS_COUNT;
            redraw(getGridEl(x, 0)->w);
            anything = true;
        }
    }
    return anything;
}

LRESULT CALLBACK WndProcGrid(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam) {
    gridEl* s = getGridData(hWnd);
    switch (message) {
    case WM_CREATE: {
        SetTimer(hWnd, 8, 50, NULL);
        CREATESTRUCT* pCreate = reinterpret_cast<CREATESTRUCT*>(lParam);
        gridEl* pState;
        pState = reinterpret_cast<gridEl*>(pCreate->lpCreateParams);

        SetWindowLongPtr(hWnd, GWLP_USERDATA, (LONG_PTR)pState);
        break;
    }
    case WM_ERASEBKGND:
        return 1;
    case WM_MOUSEMOVE: {
        trackMouse(hWnd);
        s->hover = true;
        s->tx = s->x - 5;
        s->ty = s->y - 5;
        s->ts = s->s + 10;
        MoveWindow(hWnd, s->tx, s->ty, s->ts, s->ts, TRUE);
        break;
    }
    case WM_PAINT: {
        RECT rc;
        PAINTSTRUCT ps;
        GetClientRect(hWnd, &rc);
        HDC dc = BeginPaint(hWnd, &ps);
        if (s->active) {
            if (s->empty) FillRect(dc, &rc, brushesCrossed[s->color]);
            else if (chosen.x == s->gridX && chosen.y == s->gridY) {
                FillRect(dc, &rc, brushes[0]);
                rc.left += 4;
                rc.top += 4;
                rc.right -= 4;
                rc.bottom -= 4;
                FillRect(dc, &rc, brushes[s->color]);
            }
            else {
                FillRect(dc, &rc, brushes[s->color]);
            }
        }
        else {
            FillRect(dc, &rc, brushes[0]);
        }
        EndPaint(hWnd, &ps);
        break;
    }
    case WM_MOUSELEAVE: {
        s->hover = false;
        break;
    }
    case WM_TIMER: {
        if (wParam == 8 && !s->hover && s->ts != s->s) {
            s->ts -= 2;
            s->tx++;
            s->ty++;
            MoveWindow(hWnd, s->tx, s->ty, s->ts, s->ts, TRUE);
        }
        if (wParam == 9) {
            KillTimer(hWnd, 9);
            s->active = true;
            redraw(hWnd);
        }
        break;
    }
    case WM_LBUTTONDOWN: {
        if (gameState != STATE_ACTIVE) break;
        if (chosen.x == -1 && chosen.y == -1) {
            chosen = { s->gridX, s->gridY };
            redraw(hWnd);
        }
        else {
            POINT old = chosen;
            chosen = { -1, -1 };
            redraw(getGridEl(old.x, old.y)->w);
            if (abs(old.x - s->gridX) <= 1 && abs(old.y - s->gridY) <= 1) {
                int c = getGridEl(old.x, old.y)->state->color;
                getGridEl(old.x, old.y)->state->color = s->color;
                s->color = c;
                if (checkSeries()) {
                    gameState = STATE_FIREWORKS;
                    redraw(mainWindow);
                    SetTimer(mainWindow, 5, 500, NULL);
                }
                else {
                    c = getGridEl(old.x, old.y)->state->color;
                    getGridEl(old.x, old.y)->state->color = s->color;
                    s->color = c;
                }
            }
        }
        break;
    }
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam) {

    switch (message) {
    case WM_COMMAND: {
        int wmId = LOWORD(wParam);
        HMENU hMenu = GetMenu(hWnd);

        switch (wmId) {
        case ID_GAME_NEWGAME:
            if (gameState == STATE_INITIALISATION) break;
            newGame();
            break;
        case ID_BOARDSIZE_1:
        case ID_BOARDSIZE_2:
        case ID_BOARDSIZE_3:
            if (gameState == STATE_INITIALISATION) break;
            selectBoardMenu(hMenu, wmId);
            gridsize = wmId - ID_BOARDSIZE_1 + 1;
            newBoard();
            break;
        case IDM_ABOUT:
            DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
            break;
        case ID_HELP_DEBUG:
            debugIsOn = !debugIsOn;
            CheckMenuItem(hMenu, ID_HELP_DEBUG, debugIsOn ? MF_CHECKED : MF_UNCHECKED);
            break;
        case IDM_EXIT:
            DestroyWindow(hWnd);
            break;
        default:
            return DefWindowProc(hWnd, message, wParam, lParam);
        }
    } break;
    case WM_TIMER: {
        if (wParam == 6) {
            KillTimer(hWnd, 6);

            if (checkSeries()) {
                gameState = STATE_FIREWORKS;
                SetTimer(hWnd, 5, 500, NULL);
            }
            else {
                gameState = STATE_ACTIVE;
            }
            redraw(mainWindow);
        }
        if (wParam == 5) {
            if (gameState == STATE_INITIALISATION) {
                KillTimer(hWnd, 5);
                break;
            }
            if (!fallDown()) {
                KillTimer(hWnd, 5);
                SetTimer(hWnd, 6, 0, NULL);
            }
        }
        break;
    }
    case WM_PAINT: {
        if (gameState == STATE_FIREWORKS) {
            PAINTSTRUCT ps;
            RECT rc;
            GetClientRect(hWnd, &rc);
            HDC dc = BeginPaint(hWnd, &ps);
            FillRect(dc, &rc, brushes[0]);
            EndPaint(hWnd, &ps);
        }
        else {
            return DefWindowProc(hWnd, message, wParam, lParam);
        }
        break;
    }
    case WM_SYSCOMMAND: {
        int wmId = LOWORD(wParam);

        switch (wmId) {
        case SC_MAXIMIZE:
            return 0;
        default:
            return DefWindowProc(hWnd, message, wParam, lParam);
        }
    }
    case WM_SIZING: {
        redraw(mainWindow);
        break;
    }
    case WM_NCLBUTTONDBLCLK:
        return 0;
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}

LRESULT CALLBACK WndProcParticles(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam) {
    static HDC offDC = NULL;
    static HBITMAP offOldBitmap = NULL;
    static HBITMAP offBitmap = NULL;
    static HBRUSH whiteBrush = CreateSolidBrush(RGB(255, 255, 255));
    static HFONT font = CreateFont(-96, 0, 0, 0, FW_BOLD, FALSE, FALSE, 0, EASTEUROPE_CHARSET, OUT_DEFAULT_PRECIS,
        CLIP_DEFAULT_PRECIS, NONANTIALIASED_QUALITY, DEFAULT_PITCH | FF_SWISS, NULL);
    RECT rc;
    GetClientRect(hWnd, &rc);

    switch (message) {
    case WM_CREATE: {
        HDC hdc = GetDC(hWnd);
        offDC = CreateCompatibleDC(hdc);
        offBitmap = CreateCompatibleBitmap(hdc, rc.right - rc.left, rc.bottom - rc.top);
        SelectObject(offDC, offBitmap);
        ReleaseDC(hWnd, hdc);
        SetTimer(hWnd, 4, 20, NULL);
        break;
    }
    case WM_TIMER: {
        redraw(hWnd);
        break;
    }
    case WM_ERASEBKGND:
        return 1;
    case WM_PAINT: {
        PAINTSTRUCT ps;
        HDC hdc = BeginPaint(hWnd, &ps);

        FillRect(offDC, &rc, whiteBrush);

        auto it = particleList.begin();
        while (it != particleList.end()) {
            RECT rt = { it->x, it->y, it->x + 10, it->y + 10 };
            FillRect(offDC, &rt, brushes[it->color]);
            it->x += it->dx;
            it->y += it->dy;
            if (it->x < rc.left - 10 || it->y < rc.top - 10 || it->x > rc.right || it->y > rc.bottom) {
                it = particleList.erase(it);
            }
            else {
                it++;
            }
        }

        SetBkMode(offDC, TRANSPARENT);
        SetTextColor(offDC, RGB(255, 0, 0));

        if (debugIsOn) {
            RECT rt = rc;
            rt.bottom = 200;
            HFONT oldFont = (HFONT)SelectObject(offDC, font);
            WCHAR buff[32];
            swprintf(buff, 32, L"Particles: %d", (int)particleList.size());
            DrawText(offDC, buff, (int)wcslen(buff), &rt, DT_CENTER | DT_VCENTER | DT_SINGLELINE);
        }

        BitBlt(hdc, 0, 0, rc.right, rc.bottom, offDC, 0, 0, SRCCOPY);
        EndPaint(hWnd, &ps);
        break;
    }
    case WM_SYSCOMMAND: {
        int wmId = LOWORD(wParam);

        switch (wmId) {
        case SC_MAXIMIZE:
            return 0;
        default:
            return DefWindowProc(hWnd, message, wParam, lParam);
        }
    }
    case WM_DESTROY:
        if (offOldBitmap != NULL) {
            SelectObject(offDC, offOldBitmap);
        }
        if (offDC != NULL) {
            DeleteDC(offDC);
        }
        if (offBitmap != NULL) {
            DeleteObject(offBitmap);
        }
        break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}

INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam) {
    UNREFERENCED_PARAMETER(lParam);
    switch (message) {
    case WM_INITDIALOG:
        return (INT_PTR)TRUE;

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL) {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        break;
    }
    return (INT_PTR)FALSE;
}
