using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IMenuCategory
{
    string PathName { get; }
    void OnActivate();
    void WhenClicked();
    void Click();
}
