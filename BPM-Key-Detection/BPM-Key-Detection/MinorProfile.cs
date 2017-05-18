using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM_Key_Detection
{
    //Object: Musical note distribution for minor keys
    class MinorProfile : KeyProfile
    {
        public MinorProfile()
        {
            this.Profile = new double[]
            {
            6.33,
            2.68,
            3.52,
            5.38,
            2.6,
            3.53,
            2.54,
            4.75,
            3.98,
            2.69,
            3.34,
            3.17

            //7.00255045060284420089,
            //3.14360279015996679775,
            //4.35904319714962529275,
            //5.40418120718934069657,
            //3.67234420879306133756,
            //4.08971184917797891956,
            //3.90791435991553992579,
            //6.19960288562316463867,
            //3.63424625625277419871,
            //2.87241191079875557435,
            //5.35467999794542670600,
            //3.83242038595048351013,
            };
        }
    }
}
