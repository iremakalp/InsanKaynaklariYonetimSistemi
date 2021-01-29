using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yazgel
{
    public class IkiliAramaAgaciDugumu
    {
        // referans https://www.slideshare.net/DenizKILIN/yzm-2116-blm-7-tree-ve-binary-tree-kili-aa
        public object veri;
        public IkiliAramaAgaciDugumu sol;
        public IkiliAramaAgaciDugumu sag;

        public IkiliAramaAgaciDugumu()
        {

        }
        public IkiliAramaAgaciDugumu(object veri)
        {
            this.veri = veri;
            sol = null;
            sag = null;
        }

    }
}
