var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/employees/getall/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [

            { "data": "fullName", "width": "50%" },
            { "data": "overallProgress", "width": "50%" },

            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/employees/Update?id=${data}" class='btn btn-info text-white' style='cursor:pointer; width:70px;'>
                            <i class="far fa-edit"></i>
                        </a>
                        &nbsp;
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
                            onclick=Delete('/employees/Delete?id='+${data})>
                        <i class="fas fa-user-minus"></i>
                           
                        </a>
                        </div>`;
                }, "width": "40%"
            }
        ],
        "language": {
            "emptyTable": "There are no employees yet. Try creating one!"
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this item.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}