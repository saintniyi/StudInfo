
var dataTable;

$(document).ready(function () {
    loadRoles();
})



function loadRoles() {
    dataTable = $("#tblRoles").DataTable({
        ajax: "/Role/GetRolesList",
        columns: [
            { data: "id" },
            { data: "roleName" },
            {
                data: "id",
                render: function (data) {
                    return `
                             <div class="w-75 btn-group" role="group">
                                <a href="/Role/Upsert?id=${data}"
                                class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>&nbsp;Edit</a>
                                <a onClick=Remove('/Role/DeleteRole/${data}')
                                class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i>&nbsp;Delete</a>
					        </div>
                        `
                }
            }
        ]
    })
}




function Remove(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.sno == 2) {
                        toastr.success(data.message);

                        setTimeout(function () {
                            window.location.reload()
                        }, 1500)
                    }
                    else if (data.sno == 1 || data.sno == 3) {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}










