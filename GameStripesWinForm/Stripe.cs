using System.Drawing;
using System.Windows.Forms;

namespace GameStripes
{
    public class Stripe : Control
    {
        public Stripe() { }

        public Stripe(int posX, int posY, int width, int height, Color color)
        {
            Size = new Size(width, height);
            Location = new Point(posX, posY);
            BackColor = color;
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (!PoolStripe.InstancePoolStripe.HasIntersectionsWithStripes(this))
            {
                PoolStripe.InstancePoolStripe.RemoveStripe(this);
                Dispose();
            }
        }

    }
    
}
