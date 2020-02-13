$.fn.dataTable.Api.register('order.neutral()', function () {
    return this.iterator('table', function (s) {
        s.aaSorting.length = 0;
        s.aiDisplay.sort(function (a, b) {
            return a - b;
        });
        s.aiDisplayMaster.sort(function (a, b) {
            return a - b;
        });
    });
});