using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace ncl
{
    namespace Equipment
    {
        public enum Mode { StandBy, Manual, SemiAuto, Auto, Setup, Tenkey, Cycle };

        public enum Status { Idle, Error, Warning, Run, Stop };

        public enum Authority { Operator, Engineer, Administrator, Developer };
    }
}
