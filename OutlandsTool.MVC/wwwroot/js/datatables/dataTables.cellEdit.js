/*! CellEdit 1.0.19
 * ©2016 Elliott Beaty - datatables.net/license
 */
//This library has been extended beyond the point of it being anything remotely like the original celledit.
//Do not attempt to upgrade this version!

jQuery.fn.dataTable.Api.register('MakeCellsEditable()', function (settings) {
    //console.log('datatables edit called');
    var table = this.table();

    jQuery.fn.extend({
        // UPDATE
        updateEditableCell: function (callingElement) {

        },
        //Bypassed default plugin update
        updateEditableCells: function (callingElement) {
            if (callingElement === 'undefined') {
                return;
            }
            // Need to redeclare table here for situations where we have more than one datatable on the page. See issue6 on github
            var table = $($(callingElement).closest("table")).DataTable().table();
            //Loop through selected Rows
            $.each(table.rows('.selected').indexes(), function (index, element) {
                //Loop through edit columns from settings
                $.each(settings.columns, function (i, val) {
                    var visible = true;
                    try {
                        $.each(settings.inputTypes,
                            function (index, setting) {
                                if (setting.column === val) {
                                    var inputSetting = setting;
                                    if (typeof inputSetting.edit !== "undefined") {
                                        visible = false;
                                        //get column values
                                    }
                                }
                            });  
                    } catch (e) {
                        console.log('errors ' + e);
                    } 
                   
                    //console.log("vis: " + visible);
                    if (visible) {

                        var td = table.cell({ row: element, column: val }).node();
                        var cell = table.cell({ row: element, column: val });
                        var inputField = getInputField(td);
                        var newValue = inputField.val();
                        //console.log("val: " + table.cell({ row: element, column: val }).data());
                        _update(cell, newValue);
                    }
                });

                $(element).removeClass('selected');
            });
            // Update datatable columns from inputs
            function _update(cell, newValue) {
                //console.log("cell" + cell + " value: " + newValue);
                cell.data(newValue);
            }
            //var currentPageIndex = table.page.info().page;

            //Redraw table
            //table.page(currentPageIndex).draw(false);
        },
        updateAllRows: function () {
            //Loop through selected Rows
            $.each(table.rows().indexes(), function (index, element) {
                //Loop through edit columns from settings
                $.each(settings.columns, function (i, val) {
                    {
                        //get column values
                        var td = table.cell({ row: element, column: val }).node();
                        var cell = table.cell({ row: element, column: val });
                        var inputField = getInputField(td);
                        var newValue = inputField.val();
                        //console.log("newval: " + newValue);
                        _update(cell, newValue);
                    }
                });
                $(element).removeClass('selected');
            });
            // Update datatable columns from inputs
            function _update(cell, newValue) {
                //console.log("cell" + cell + " value: " + newValue);
                cell.data(newValue);
            }
        },
        AddNewRow: function (callingElement, row, tablename) {
            if (callingElement === 'undefined') {
                //console.log('undefined');
                return;
            }
            if (typeof tablename === 'undefined') {
                tablename = '#users-table > tbody';
            }
            $(row).addClass('selected');
            $(tablename).prepend(row);
        },
        SubmitNewRow: function (callingElement) {
            if (callingElement === 'undefined') {
                return;
            }
            var input = getInputField(this);
        },
        ShowEditRow: function (e) {
            var table = $($(e).closest("table")).DataTable().table();
            var controlRow = $(e);
            var rowId = 0;
            try {
                rowId = table.row(e).data().Id;
            } catch (e) {
                console.log("error, cant find rowid");
            }

            // DETERMINE WHAT COLUMNS CAN BE EDITED From CellEdit Settings
            $.each(settings.columns, function (i, val) {
                var cell = table.cell({ row: controlRow, column: val });
                var updatecell = $(controlRow).children('td')[val];
                var currentColumnIndex = cell.index().column;
                var oldValue = table.cell(updatecell).data();
                //console.log("edit cell: " + oldValue);

                //look for json formatted dates
                try {
                    if (isNaN(oldValue)) {
                        if (oldValue.indexOf("/Date(") >= 0) {
                            oldValue = dtConvFromJSON(oldValue);
                        }
                    }
                }
                catch (err) {
                    //convert date to normal format
                    oldValue = dtConvFromJSON(oldValue);
                }
                // Sanitize value
                oldValue = sanitizeCellValue(oldValue);

                var visible = true;
                try {
                    $.each(settings.inputTypes, function (index, setting) {
                        if (setting.column === currentColumnIndex) {
                            var inputSetting = setting;
                            if (typeof inputSetting.edit !== "undefined") {
                                // Do not show edit control
                                visible = false;
                            }
                        }
                    });
                } catch (e) {
                    console.log('errors ' + e);
                } 
                
                if (visible) {
                    if (!$(cell).find('input').length && !$(cell).find('select').length && !$(cell).find('textarea').length) {
                        //console.log("date" + oldValue);
                        var input = getInputHtml(currentColumnIndex, settings, oldValue, val);
                        $(updatecell).html(input.html);
                    }
                }
            });
        },
        ShowAddRow: function (e) {
            var table = $($(e).closest("table")).DataTable().table();
            var htmlTable = $($(e).closest("table"));
            var controlRow = $(e);
            
            var row = '';
            row += '<tr id="newrow" role="row" class="selected">';
            row += ('<td><input type="checkbox" class="dt-checkboxes"></td>');

            // DETERMINE WHAT COLUMNS CAN BE Added From CellEdit Settings
            $.each(settings.columns, function (i, val) {
                var cell = table.cell({ row: controlRow, column: val });
                var currentColumnIndex = cell.index().column;
                var columns = table.settings().init().columns;
                //var columnName = columns[currentColumnIndex].data;
                var oldValue = '';
                var input = '';
                $.each(settings.inputTypes, function (index, setting) {
                    if (setting.column === currentColumnIndex) {
                        var inputSetting = setting;
                        if (typeof inputSetting.add !== "undefined") {
                            row += '<td></td>';
                        } else {
                            if (!$(cell).find('input').length && !$(cell).find('select').length && !$(cell).find('textarea').length) {
                                input = getInputHtml(currentColumnIndex, settings, oldValue, val);
                                row += '<td>' + input.html + '</td>';
                            }
                        }
                    }
                });
            });
            row += '<td><a id="AddNewRow" href="#" onclick="event.preventDefault(); SaveAddRow(this); return false;">' +
                '<i class="glyphicon glyphicon-floppy-disk glyphicon-margin"' +
                'style="color: green" data-toggle="tooltip" data-original-title="Add"></i></a >&nbsp; ' +
                '<a id="CancelAddNewRow" href="#" onclick="event.preventDefault(); CancelAddNewRow(this); return false;" >' +
                '<i class="glyphicon-margin glyphicon glyphicon-remove glyphicon-margin"' +
                'style="color: red" data-toggle="tooltip" data-original-title="Cancel"></i></a></td></tr>';
            $(htmlTable).prepend(row);
            $("htmlTable").eq(0).children("tr").eq(0).addClass('selected');
            //console.log(row_data);
        },
        UnselectRow: function (e) {
            //This function removes the edit controls and deselects row
            // var table = $(e.closest("table")).DataTable().table();
            var row = table.row(e).node();
            var index = table.row(row).index();
            $.each(settings.columns, function (i, val) {
                {
                    //get column values
                    var td = table.cell({ row: index, column: val }).node();
                    var cell = table.cell({ row: index, column: val });
                    _update(cell, table.cell(td).data());
                }
            });
            $(e).removeClass('selected');
            // Update datatable columns from inputs
            function _update(cell, newValue) {
                cell.data(newValue);
            }
        },
        UnselectAllRows: function (e) {
            try {
                $(e).removeClass('selected');

                table
                    .row(e)
                    .remove()
                    .draw();
                table
                    .row(e)
                    .add()
                    .draw();
            }
            catch (e) {
                console.log("error");
            }
        },
        GetSettingsColumns: function (e) {
            var settingsColumns = [];
            $.each(settings.columns, function (i, val) {
                settingsColumns.push(val);
            });
            return settingsColumns;
        },
        IsEditBlurEvents: function (e) {
            if (settings.editblurevents) {
                return true;
            }
            return false;
        }
    });

    // Destroy
    if (settings === "destroy") {
        $(table.body()).off("click", "td");
        table = null;
    }

});

function getInputHtml(currentColumnIndex, settings, oldValue, val) {
    var inputSetting,
        inputType,
        inputName,
        inputUrl,
        input,
        inputCss,
        confirmCss,
        cancelCss,
        minLength,
        maxLength,
        required
        ;

    input = { "focus": true, "html": null };

    if (settings.inputTypes) {
        $.each(settings.inputTypes, function (index, setting) {
            if (setting.column === currentColumnIndex) {
                inputSetting = setting;
                if (typeof inputSetting.type !== "undefined") {
                    inputType = inputSetting.type.toLowerCase();
                }
                if (typeof inputSetting.name !== "undefined") {
                    inputName = inputSetting.name.toLowerCase();
                }
                if (typeof inputSetting.url !== "undefined") {
                    inputUrl = inputSetting.url.toLowerCase();
                }
                if (typeof inputSetting.css !== "undefined") {
                    inputCss = inputSetting.css.toLowerCase();
                }
                if (typeof inputSetting.minLength !== "undefined") {
                    minLength = inputSetting.minLength;
                }
                if (typeof inputSetting.maxLength !== "undefined") {
                    maxLength = inputSetting.maxLength;
                }
            }
        });
    }

    //if (settings.inputCss) { inputCss = settings.inputCss; }
    if (settings.confirmationButton) {
        confirmCss = settings.confirmationButton.confirmCss;
        cancelCss = settings.confirmationButton.cancelCss;
        inputType = inputType + "-confirm";
    }

    switch (inputType) {

        case "list":
            //The parameter sent to the controller is select plus list index
            //multiple dropdowns can be specified by using switch statement on controller method
            var options;
            $.ajax({
                type: "POST",
                url: inputUrl,
                dataType: "json",
                data: { selectlist: inputName },
                success: function (result) {
                    input.html = "<select id='" + inputName + "' class='ui-corner-all' minlength='" + minLength + " maxlength='" + maxLength + "' >";
                    $.each(result, function (i, item) {
                        if (oldValue === item.text) {
                            input.html += '<option  selected="selected" value="' +
                                item.value + '">' + item.text + '</option>';
                        } else {
                            input.html += '<option value="' + item.value + '">' + item.text + '</option>';
                        }
                    });
                    //console.log(input.html);
                },
                async: false
            });
            input.html += "</select>";
            input.focus = false;
            break;

        case "list-confirm": // List w/ confirm
            input.html = "<select class='" + inputCss + "'>";
            $.each(inputSetting.options, function (index, option) {
                input.html = input.html + "<option value='" + option.value + "' >" + option.display + "</option>";
            });
            input.html = input.html + "</select>";
            input.focus = false;
            break;
        case "datepicker": //Both datepicker options work best when confirming the values
        case "datepicker-confirm":
            // Makesure jQuery UI is loaded on the page
            if (typeof jQuery.ui === 'undefined') {
                alert("jQuery UI is required for the DatePicker control but it is not loaded on the page!");
                break;
            }
            jQuery(".datepick").datepicker("destroy");
            try {
                if (settings.editblurevents) {
                    input.html = "<input id='datecelledit" + val + "' type='text' name='datatabledate" + val + "' class='datepick ui-corner-all " + inputCss +"' value='" + oldValue + "' onblur='CellEditBlur(this)'></input>";
                }
                else {
                    input.html = "<form id='form" + val + "'><input id='datecelledit" + val + "' type='text' name='datatabledate" + val + "' data-val='true' class='datepick ui-corner-all'   value='" + oldValue + "'></input><form>";
                }
            } catch (e) {
                input.html = "<form id='form" + val + "'><input id='datecelledit" + val + "' type='text' name='datatabledate" + val + "' class='datepick ui-corner-all'   value='" + oldValue + "'></input><form>";
            }
            setTimeout(function () { //Set timeout to allow the script to write the input.html before triggering the datepicker 
                var icon = "/braidss/images/calendar.gif";
                // Allow the user to provide icon 
                if (typeof inputSetting.options !== 'undefined' && typeof inputSetting.options.icon !== 'undefined') {
                    icon = inputSetting.options.icon;
                }
                var self = jQuery('.datepick').datepicker();
            }, 100);
            break;
        case "text-confirm": // text input w/ confirm
            input.html = "<input id='ejbeatycelledit' class='" + inputCss + "' minlength='" + minLength + " maxlength='" + maxLength + "' value='" + oldValue + "' onblur='CellEditBlur(this)'></input> ";
            break;
        case "undefined-confirm": // text input w/ confirm
            input.html = "<input id='ejbeatycelledit' class='" + inputCss + "' minlength='" + minLength + " maxlength='" + maxLength + "' value='" + oldValue + "' onblur='CellEditBlur(this)'></input></a> ";
            break;
        case "textarea":
        case "textarea-confirm":
            input.html = "<textarea id='rowedittextarea'>" + oldValue + "</textarea>";
            break;
        default: // text input
            try {
                if (settings.editblurevents) {
                    input.html = "<input id='" + inputName + "' class='" + inputCss + "'  minlength='" + minLength + "' maxlength='" + maxLength + "' name='" + inputName + "' value='" + oldValue + "' onblur='CellEditBlur(this)' ></input>";
                }
                else {
                    input.html = "<input id='" + inputName + "' class='" + inputCss + "'  minlength='" + minLength + "' + required maxlength='" + maxLength + "' name='" + inputName + "' + value='" + oldValue + "' ></input>";
                }
                break;
            } catch (e) {
                input.html = "<input id='" + inputName + "' class='" + inputCss + "'  minlength='" + minLength + "' maxlength='" + maxLength + "' name='" + inputName + "' value='" + oldValue + "' ></input>";
            }
    }
    return input;
}

function getInputField(callingElement) {
    // Get Edit column control type

    var inputField;
    var controltype = "";
    try {
        controltype = $(callingElement).children("input:text,select,textarea").prop('nodeName').toLowerCase();
    }
    catch (e) {
        console.log("error getting inputfield: ");
    }
    switch (controltype) {
        case 'input':
            if ($(callingElement).children('input').length > 0) {
                inputField = $(callingElement).children('input');
            }
            break;
        case 'select':
            if ($(callingElement).children('select').length > 0) {
                inputField = $(callingElement).children('select');
            }
            break;
        case 'textarea':
            if ($(callingElement).children('textarea').length > 0) {
                inputField = $(callingElement).children('textarea');
            }
            break;

        default:
            inputField = $(callingElement);
    }
    return inputField;
}

function sanitizeCellValue(cellValue) {

    if (typeof (cellValue) === 'undefined' || cellValue === null || cellValue.length < 1) {

        return "";
    }

    // If not a number
    if (isNaN(cellValue)) {
        // escape single quote
        cellValue = cellValue.replace(/'/g, "&#39;");
    }
    return cellValue;
}
