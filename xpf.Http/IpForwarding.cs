using System;
using System.Linq;
using xpf.Http.Original;

namespace xpf.Http
{
    public class IpForwarding : IRequireNavigationContext
    {
        public IpForwarding(NavigationContext parent)
        {
            ((IRequireNavigationContext)this).NavigationContext = parent;
            this.Parent = parent;
        }


        public NavigationContext Austrlia
        {
            get
            {
                int ipaddresss = this.GetRandomAddress(1704984576, 1707081727);
                return this.SetHeaders(this.ConvertToString(ipaddresss));
            }
        }

        public NavigationContext UnitedKingdom
        {
            get
            {
                int ipaddresss = this.GetRandomAddress(855638016, 872415231);
                return this.SetHeaders(this.ConvertToString(ipaddresss));
            }
        }

        public NavigationContext UnitedStates
        {
            get
            {
                int ipaddresss = this.GetRandomAddress(251658240, 268435455);
                return this.SetHeaders(this.ConvertToString(ipaddresss));
            }
        }

        NavigationContext SetHeaders(string ipaddress)
        {
            // While X-Forwarded-For is the most useful, other headers may be used by some servers
            this.Parent.Model.Headers.Add(new HttpHeader { Key = "Client-Ip", Value = new[] { ipaddress } });
            this.Parent.Model.Headers.Add(new HttpHeader { Key = "X-Forwarded-For", Value = new[] { ipaddress } });
            this.Parent.Model.Headers.Add(new HttpHeader { Key = "X-Forwarded", Value = new[] { ipaddress } });
            this.Parent.Model.Headers.Add(new HttpHeader { Key = "X-Cluster-Client-Ip", Value = new[] { ipaddress } });
            this.Parent.Model.Headers.Add(new HttpHeader { Key = "Forwarded-For", Value = new[] { ipaddress } });
            this.Parent.Model.Headers.Add(new HttpHeader { Key = "Forwarded", Value = new[] { ipaddress } });
            return this.Parent;
        }

        NavigationContext Parent { get; set; }
        NavigationContext IRequireNavigationContext.NavigationContext { get; set; }


        int GetRandomAddress(int rangeStart, int rangeEnd)
        {
            return new Random().Next(rangeStart, rangeEnd);
        }

        int ConvertIpToInteger(string ipaddress)
        {

            var ip = ipaddress.Split('.').Select(Byte.Parse).ToArray();
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(ip);
            }
            var num = BitConverter.ToInt32(ip, 0);

            return num;
        }

        public string ConvertToString(int ipaddress)
        {
            var ip = BitConverter.GetBytes(ipaddress);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(ip);
            }
            var address = String.Join(".", ip.Select(n => n.ToString()));

            return address;
        }
    }
}