//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  Piotr Duperas
using System;
using System.Text;


namespace TravelAgencies.DataAccess
{
    
    public interface IDecryptable<T>
    {
        IIterator<T> GetDecryptedIterator();
    }

    public interface IEncryptor<T>
    {
        T Encrypt(T t);
        T Decrypt(T t);
    }

    public abstract class ChainableCodec<T> : IEncryptor<T>
    {
        public IEncryptor<T> next { get; set; }
        public IEncryptor<T> prev { get; set; }
        protected abstract T GetEncryption(T t);
        protected abstract T GetDecryption(T t);
        public T Encrypt(T t)
        {
            T res = GetEncryption(t);
            if (next == null) return res;
            return next.Encrypt(res);
        }

        public T Decrypt(T t)
        {
            T res = GetDecryption(t);
            if (prev == null) return res;
            return prev.Decrypt(res);
        }
    }

    public class ChainEncryptor<T> : IEncryptor<T>
    {
        private ChainableCodec<T> end;
        private ChainableCodec<T> start;

        public ChainEncryptor<T> Join(ChainableCodec<T> codec)
        {
            if(start == null)
            {
                start = end = codec;
                return this;
            }
            codec.prev = end;
            end.next = codec;
            end = codec;
            return this;
        }

        public T Encrypt(T t)
        {
            return start.Encrypt(t);
        }

        public T Decrypt(T t)
        {
            return end.Decrypt(t);
        }
    }

    public abstract class CodecWithEncryptor<T, U> : IEncryptor<T>
    {
        protected IEncryptor<U> cipher;
        public CodecWithEncryptor(IEncryptor<U> cipher)
        {
            this.cipher = cipher;
        }

        public abstract T Decrypt(T t);
        public abstract T Encrypt(T t);
    }

    public class ReverseStringCodec : ChainableCodec<string>
    {
        private static string Reverse(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        protected override string GetDecryption(string s)
        {
            return Reverse(s);
        }

        protected override string GetEncryption(string s)
        {
            return Reverse(s);
        }
    }

    public class FrameStringCodec : ChainableCodec<string>
    {
        int n;
        public FrameStringCodec(int n)
        {
            this.n = n;
        }

        protected override string GetDecryption(string s)
        {
            if (s.Length < 2 * n) return null;

            return s.Substring(n, s.Length - 2 * n);
        }

        protected override string GetEncryption(string s)
        {
            StringBuilder sb = new StringBuilder();

            for(int i = 1; i <= n; i++)
            {
                sb.Append(i);
            }

            sb.Append(s);

            for (int i = n; i > 0; i--)
            {
                sb.Append(i);
            }
            return sb.ToString();
        }
    }

    public class CezarStringCodec : ChainableCodec<string>
    {
        int n;
        public CezarStringCodec(int n)
        {
            this.n = n;
        }

        private string Cezar(string s, int m)
        {

            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < s.Length; i++)
            {
                int c = int.Parse(s[i].ToString()) + m;
                c = (c % 10 + 10) % 10;
                sb.Append(c.ToString());
            }

            return sb.ToString();
        }

        protected override string GetDecryption(string s)
        {
            return Cezar(s, -n);
        }

        protected override string GetEncryption(string s)
        {
            return Cezar(s, n);
        }
    }

    public class SwapStringCodec : ChainableCodec<string>
    {
        private string Swap(string s)
        {

            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < s.Length - 1; i += 2)
            {
                sb.Append(s[i + 1]);
                sb.Append(s[i]);
            }
            if (s.Length % 2 == 1) sb.Append(s[s.Length - 1]);

            return sb.ToString();
        }

        protected override string GetDecryption(string s)
        {
            return Swap(s);
        }

        protected override string GetEncryption(string s)
        {
            return Swap(s);
        }
    }

    public class PushStringCodec : ChainableCodec<string>
    {
        int n;
        public PushStringCodec(int n)
        {
            this.n = n;
        }

        private string Push(string s, int m)
        {

            int len = s.Length;
            char[] chars = new char[len];
            for (int i = 0; i < len; i++)
            {
                chars[((i + m) % len + len) % len] = s[i];
            }

            return new string(chars);
        }

        protected override string GetDecryption(string s)
        {
            return Push(s, -n);
        }

        protected override string GetEncryption(string s)
        {
            return Push(s, n);
        }
    }
}
