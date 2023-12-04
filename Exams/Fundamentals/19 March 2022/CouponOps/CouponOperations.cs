namespace CouponOps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CouponOps.Models;
    using Interfaces;

    public class CouponOperations : ICouponOperations
    {
        private Dictionary<string, Coupon> coupons = new Dictionary<string, Coupon>();
        private Dictionary<string, Website> websites = new Dictionary<string, Website>();
        private Dictionary<string, HashSet<Coupon>> websiteCoupons = new Dictionary<string, HashSet<Coupon>>();

        public void AddCoupon(Website website, Coupon coupon)
        {
            if (!this.Exist(website))
                throw new ArgumentException();

            coupons.Add(coupon.Code, coupon);
            websiteCoupons[website.Domain].Add(coupon);
        }

        public bool Exist(Website website)
            => this.GetSites().Contains(website);

        public bool Exist(Coupon coupon)
            => this.coupons.Values.Contains(coupon);

        public IEnumerable<Coupon> GetCouponsForWebsite(Website website)
        {
            if (!websites.ContainsKey(website.Domain))
                throw new ArgumentException();

            return websiteCoupons[website.Domain];
        }

        public IEnumerable<Coupon> GetCouponsOrderedByValidityDescAndDiscountPercentageDesc()
            => coupons.Values.OrderByDescending(c => c.Validity)
                  .ThenByDescending(c => c.DiscountPercentage);

        public IEnumerable<Website> GetSites()
            => websites.Values;

        public IEnumerable<Website> GetWebsitesOrderedByUserCountAndCouponsCountDesc()
            => this.GetSites().OrderBy(w => w.UsersCount)
                .ThenByDescending(w => websiteCoupons[w.Domain].Count);

        public void RegisterSite(Website website)
        {
            if(this.Exist(website))
                throw new ArgumentException();

            websites.Add(website.Domain, website);
            websiteCoupons.Add(website.Domain, new HashSet<Coupon>());
        }

        public Coupon RemoveCoupon(string code)
        {
            if (!coupons.ContainsKey(code))
                throw new ArgumentException();

            var couponToRemove = coupons[code];
            foreach (var c in websiteCoupons)
            {
                if(c.Value.Contains(couponToRemove))
                    c.Value.Remove(couponToRemove);
            }

            coupons.Remove(code);
            return couponToRemove;
        }

        public Website RemoveWebsite(string domain)
        {
            if (!websites.ContainsKey(domain))
                throw new ArgumentException();

            var websiteToDelete = websites[domain];           
            websites.Remove(domain);
            websiteCoupons.Remove(domain);

            return websiteToDelete;
        }

        public void UseCoupon(Website website, Coupon coupon)
        {
            if(!websites.ContainsKey(website.Domain) || !coupons.ContainsKey(coupon.Code))
                throw new ArgumentException();
            if (!websiteCoupons[website.Domain].Contains(coupon))
                throw new ArgumentException();

            websiteCoupons[website.Domain].Remove(coupon);
            coupons.Remove(coupon.Code);
        }
    }
}
