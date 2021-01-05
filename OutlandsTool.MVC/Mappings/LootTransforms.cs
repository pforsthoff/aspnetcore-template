using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlandsTool.MVC.Mappings
{
    public static class LootTransforms
    {
        public static string ProcessServerActionsAnchor(int id)
        {
            var sb = new StringBuilder();

            sb.AppendFormat(
                "<a id='{0}' href='#' onclick='event.preventDefault();DeleteSelectedRows(this); return false; '<i class='glyphicon-margin glyphicon glyphicon-remove-sign glyphicon-margin' style='color: red' data-toggle='tooltip' title='Delete Selected Rows'></i></a>&nbsp;", id);
            sb.AppendFormat(
                "<a id='{0}' href='#' onclick='event.preventDefault();UpdateSelectedRows(this); return false; '<i class='glyphicon-margin glyphicon glyphicon-floppy-disk glyphicon-margin' style='color: green' data-toggle='tooltip' title='Update Selected Rows'></i></a>&nbsp;", id);
            sb.AppendFormat(
                "<a id='AddRow' onclick='event.preventDefault();ShowAddRow(this); return false; ' href='#' ><span><i class='glyphicon-margin glyphicon glyphicon-plus glyphicon-margin' style='color: blue' data-toggle='tooltip' title='Add New Row'></i></span></a>");

            return sb.ToString();
        }
    }
}
