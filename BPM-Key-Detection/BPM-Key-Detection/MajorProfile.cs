using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: Musical note distribution for major keys
    class MajorProfile : KeyProfile
    {
        
        public MajorProfile()
        {
            this.Profile = new double[]
        {
            //6.35,
            //2.23,
            //3.48,
            //2.33,
            //4.38,
            //4.09,
            //2.52,
            //5.19,
            //2.39,
            //3.66,
            //2.29,
            //2.88

            7.23900502618145225142,
            3.50351166725158691406,
            3.58445177536649417505,
            2.84511816478676315967,
            5.81898892118549859731,
            4.55865057415321039969,
            2.44778850545506543313,
            6.99473192146829525484,
            3.39106613673504853068,
            4.55614256655143456953,
            4.07392666663523606019,
            4.45932757378886890365,
        };
        }
    }
}
