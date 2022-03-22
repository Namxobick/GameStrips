using System;
using System.Collections.Generic;


namespace GameStripes
{
    public class PoolStripe
    {
        private List<Stripe> _stripes;

        private int _maxCount;
        private int _clickCount = 0;
        public Action OnPoolEmpty;
        public Action OnFullPool;

        public static PoolStripe InstancePoolStripe;
        private PoolStripe(int maxCount)
        {
            _maxCount = maxCount;
            _stripes = new List<Stripe>();
        }

        public static void Init(int maxCount)
        {
            if (InstancePoolStripe == null)
                InstancePoolStripe = new PoolStripe(maxCount);
        }

        public void SetMaxCount(int maxCount)
        {
            InstancePoolStripe._maxCount = maxCount;
        }

        public void AddStripe(Stripe stripe)
        {
            if (InstancePoolStripe._stripes.Count >= _maxCount)
            {
                OnFullPool?.Invoke();
                return;
            }

            if (InstancePoolStripe._stripes.Contains(stripe) == false)
            {
                InstancePoolStripe._stripes.Add(stripe);
            }

        }

        public void RemoveStripe(Stripe stripe)
        {
            InstancePoolStripe._stripes.Remove(stripe);

            if (InstancePoolStripe._stripes.Count == 0)
            {
                OnPoolEmpty?.Invoke();
                return;
            }
        }

        public void RemoveAllStripe()
        {
            _clickCount = 0;
            InstancePoolStripe._stripes.Clear();
        }

        public int GetClickCount()
        {
            return _clickCount;
        }

        public bool HasIntersectionsWithStripes(Stripe stripe)
        {
            _clickCount++;
            foreach (var stripeInStripes in InstancePoolStripe._stripes)
            {

                var xDistanceBetweenCenters = Math.Abs((stripe.Location.X + stripe.Size.Width / 2) - (stripeInStripes.Location.X + stripeInStripes.Size.Width / 2));
                var yDistanceBetweenCenters = Math.Abs((stripe.Location.Y + stripe.Size.Height / 2) - (stripeInStripes.Location.Y + stripeInStripes.Size.Height / 2));
                var widthAverage = (stripe.Size.Width + stripeInStripes.Size.Width) / 2;
                var heightAverage = (stripe.Size.Height + stripeInStripes.Size.Height) / 2;
                
                if ((xDistanceBetweenCenters <= widthAverage) && (yDistanceBetweenCenters <= heightAverage)) // True - пересечение     
                    if (InstancePoolStripe._stripes.IndexOf(stripe) < InstancePoolStripe._stripes.IndexOf(stripeInStripes))
                        return true;   
            }
            return false;
        }
    }
}
