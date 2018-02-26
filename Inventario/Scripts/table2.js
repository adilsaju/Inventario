$(document).ready(function () {
    // Setup - add a text input to each footer cell
    $('#example tfoot th').each(function () {
        var title = $(this).text();

        $(this).html('<input type="text" />');

    });

    // DataTable
    var table = $('#example').DataTable();

    // Apply the search
    table.columns().every(function () {
        var that = this;

        $('input', this.footer()).on('keyup change', function () {
            if (that.search() !== this.value) {
                that
                    .search(this.value)
                    .draw();
            }
        });
    });
});

$('#example').dataTable({
    "columnDefs": [
      { "width": "20%", "targets": 6 }
    ]
});