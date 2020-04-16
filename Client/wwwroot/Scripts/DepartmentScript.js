$(document).ready(function () {
    $('#Department').dataTable({ //Nama table pada index
        "ajax": {
            url: "/Department/LoadDepartment", //Nama controller/fungsi pada index controller
            type: "GET",
            dataType: "json"
        },
        "columnDefs":
            [{
                "targets": [3],
                "orderadble" : false
            }],
        "columns": [
            { "data": "name" },
            {
                "data": "createDate", "render": function (data) {
                    return moment(data).format('DD MMMM YYYY, h:mm a');
                }
            },
            {
                "data": "updateDate", "render": function (data) {
                    var dateupdate = "Not Updated Yet";
                    var nulldate = null;
                    if (data == nulldate) {
                        return dateupdate;
                    } else {
                        return moment(data).format('DD MMMM YYYY, h:mm a');
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return " <td><button type='button' class='btn btn-warning' id='Update' onclick=$('#EditBtn').show();GetById('" + row.id + "');>Edit</button> <button type='button' class='btn btn-danger' id='Delete' onclick=Delete('" + row.id + "');>Delete</button ></td >";
                }
            },
        ]
    });

    // tooltip
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })
});

function Delete(Id) {
    Swal.fire({
        title: "Do you want to delete it?",
        text: "You won't be able to restore this!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        //debugger;
        if (result.value) {
            $.ajax({
                url: "/Department/Delete/" + Id,
                data: { Id: Id }
            }).then((result) => {
                if (result.statusCode == 200) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Department Deleted Succesfully'
                    }).then((result) => {
                        if (result.value) {
                            $('#Department').DataTable().ajax.reload();
                        }
                    });
                }
                else {
                    Swal.fire('Error', 'Failed to Delete Department', 'error');
                    ShowModal();
                }
            });
        }
    });
}

function Save() {
    if ($('#Name').val() == 0) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
            footer: '<a href>Please Full Fill The Department Name</a>'
        });
        $('#Department').DataTable().ajax.reload();
    }
    else {
        var Department = new Object(); // new object Department
        Department.Name = $('#Name').val();
        $.ajax({
            type: 'POST',
            url: '/Department/InsertOrUpdate/',
            data: Department
        }).then((result) => {
            //debugger;
            if (result.statusCode == 200) {
                Swal.fire({
                    position: 'center',
                    type: 'success',
                    title: 'Department Added Succesfully'
                }).then((result) => {
                    if (result.value) {
                        $('#Department').DataTable().ajax.reload();
                    }
                });
            }
            else {
                Swal.fire('Error', 'Failed to Add Department', 'error');
                ClearScreen();
            }
        });
    }
}





function ClearScreen() {
    $('#Id').val('');
    $('#Name').val('');
    $('#EditBtn').hide();
    $('#SaveBtn').show();
}




function GetById(Id) {
    $.ajax({
        url: "/Department/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            //debugger;
            $('#Id').val(result.id);
            $('#Name').val(result.name);
            $('#myModal').modal('show');
            $('#UpdateBtn').show();
            $('#SaveBtn').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


function Edit() {
    if ($('#Name').val() == 0) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
            footer: '<a href>Please Full Fill The Department Name</a>'
        });
        $('#Department').DataTable().ajax.reload();
    }
    else {
        var Department = new Object();
        Department.Id = $('#Id').val();
        Department.Name = $('#Name').val();
        $.ajax({
            type: 'POST',
            url: '/Department/InsertOrUpdate/',
            data: Department
        }).then((result) => {
            if (result.statusCode == 200) {
                Swal.fire({
                    position: 'center',
                    type: 'success',
                    title: 'Department Updated Succesfully'
                }).then((result) => {
                    if (result.value) {
                        $('#Department').DataTable().ajax.reload();
                    }
                });
            }
            else {
                Swal.fire('Error', 'Failed to Update Department', 'error');
                $('#myModal').modal('show');
                $('#SaveBtn').hide();
            }
        });
    }
}
