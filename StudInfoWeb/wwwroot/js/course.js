
var dataTable;

$(document).ready(function () {
    loadCourseTable();
})



//function loadCourseTable() {
//    dataTable = $('#tblCourse').DataTable({
//        "ajax": {
//            "url": "/Course/GetAllCourses"
//        },
//        "columns": [
//            { "data": "courseCode", "width": "13%" },
//            { "data": "courseName", "width": "27%" },
//            { "data": "courseUnit", "width": "13%" },
//            { "data": "deptCode", "width": "15%" },
//            {
//                "data": "id",
//                "render": function (data) {
//                    return `
//                        <div class="w-75 btn-group" role="group">
//                        <a href="/Course/Upsert?id=${data}"
//                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>&nbsp;Edit</a>
//                        <a onClick=Remove('/Course/DeleteCourse/${data}')
//                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i>&nbsp;Delete</a>
//					</div>
//                        `
//                },
//                "width": "32%"
//            }
//        ]
//    });
//}




function loadCourseTable() {
    dataTable = $("#tblCourse").DataTable({
        ajax: "/Course/GetAllCourses",
        columns: [
            { data: "courseCode", width: "7%" },
            { data: "courseName", width: "37%" }, 
            { data: "courseUnit", width: "6%" },
            { data: "deptCode", width: "9%" },
            { data: "level", width: "7%" },
            { data: "semester", width: "7%" },
            {
                data: "id",
                render: function (data) {
                    return `
                             
                              <div class="w-100 btn-group" role="group">

                                <a href="/Course/Upsert?id=${data}" 
                                class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>&nbsp;Edit</a>

                                <a onClick=Remove('/Course/DeleteCourse/${data}') 
                                class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i>&nbsp;Delete</a>

					        </div>
                        `
                },
                "width": "27%"
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
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}










