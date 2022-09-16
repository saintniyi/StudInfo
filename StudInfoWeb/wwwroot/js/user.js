var dataTable;

$(document).ready(function () {
    loadUsers();
})



function loadUsers() {
    dataTable = $("#tblUser").DataTable({
        ajax: "/UserManagement/GetAllUsers",
        columns: [
            { data: "id" },
            { data: "firstName" },
            { data: "lastName" },
            { data: "userName" },
            
            {
                data: "id",
                render: function (data) {
                    return `

                            <div class="w-75 btn-group" role="group">
                                <a href="/UserManagement/Edit?id=${data}"
                                class="btn btn-primary mx-2" style="width:100px"> <i class="bi bi-pencil-square"></i>&nbsp;Edit</a>
                            </div>
                        `
                }
            }
        ]
    })
}