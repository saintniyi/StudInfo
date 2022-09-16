var dataTable;

$(document).ready(function () {
    loadStudentTable();
})



function loadStudentTable() {
    dataTable = $("#tblStudent").DataTable({
        ajax: "/Student/GetAllStudents",
        columns: [
            { data: "studNos" },
            { data: "firstName" },
            { data: "lastName" },
            {
                data: "sex",
                render: function (data) {
                    return data == 0 ? "Male" : "Female"
                }
            },
            { data: "deptCode" },
            {
                data: "degreeProgramName",
                render: function (data) {
                    switch (data) {
                        case 1: return "Accounting"; break;
                        case 2: return "Business Administration"; break;
                        case 3: return "Biological Science"; break;
                        case 4: return "Computer Science"; break;
                        case 5: return "Electrical Electronic Eng"; break;
                        case 6: return "Mathematics"; break;
                        case 7: return "Medicine & Surgery"; break;
                    }
                }
            },
            {
                data: "id",
                render: function (data) {
                    return `
                             
                              <div class="btn-group" role="group">

                                <a href="/Student/Upsert?id=${data}"
                                class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>&nbsp;Edit</a>

                                <a href='/Student/Delete?id=${data}'
                                class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i>&nbsp;Delete</a>

					        </div>
                        `
                }
            }
        ]
    })
}



