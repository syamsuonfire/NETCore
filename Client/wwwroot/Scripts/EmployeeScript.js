var dateNow = new Date();
var Departments = [];
$(document).ready(function () {
$('#Employee').DataTable({ //Nama table pada index
        "ajax": {
            url: "/Employee/LoadEmployee", //Nama controller/fungsi pada index controller
            type: "GET",
            dataType: "json",
            dataSrc: ""
        },
        "columnDefs": [
            { "orderable": false, "targets": 8 },
            { "searchable": false, "targets": 8 }
        ],
        "columns": [
            {
                "data": null, render: function (data, type, row) {
                    return data.firstName + ' ' + data.lastName;
                }
            },
            { "data": "departmentName" },
            { "data": "email", "name": "Email" },
            {
                "data": "birthDate", "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            { "data": "phoneNumber", "name": "Phone Number" },
            { "data": "address", "name": "Address" },
            {
                "data": "createDate", "render": function (data) {
                    return moment(data).format('DD MMMM YYYY, h:mm a');
                }
            },
            {
                "data": "updateDate", "render": function (data) {
                    var dateupdate = "Not Updated Yet";
                    var nulldate = null;
                    if (data === nulldate) {
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
    LoadDepartment($('#DepartmentOption'));
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
        if (result.value) {
            debugger;
            $.ajax({
                url: "Employee/Delete/",
                data: { Id: Id }
            }).then((result) => {
                if (result.statusCode == 200) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Employee Deleted Succesfully'
                    }).then((result) => {
                        if (result.value) {
                            $('#Employee').DataTable().ajax.reload();
                        }
                    });
                }
                else {
                    Swal.fire('Error', 'Failed to Delete Employee', 'error');
                    ShowModal();
                }
            });
        }
    });
}


function Save() {
        var Employee = new Object(); // new object employee
        Employee.FirstName = $('#FirstName').val();
        Employee.LastName = $('#LastName').val();
        Employee.Email = $('#Email').val();
        Employee.BirthDate = $('#BirthDate').val();
        Employee.PhoneNumber = $('#PhoneNumber').val();
        Employee.Address = $('#Address').val();
        Employee.Department_Id = $('#DepartmentOption').val();

        $.ajax({
            type: 'POST',
            url: '/Employee/InsertOrUpdate/',
            data: Employee
        }).then((result) => {
            if (result.statusCode == 200) {
                Swal.fire({
                    position: 'center',
                    type: 'success',
                    title: 'Employee Added Succesfully'
                }).then((result) => {
                    if (result.value) {
                        $('#Employee').DataTable().ajax.reload();
                    }
                });
            }
            else {
                Swal.fire('Error', 'Failed to Add Employee', 'error');
                ClearScreen();
            }
        })


    }

function GetById(Id) {
    $.ajax({
        url: "/Employee/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            $('#Id').val(result[0].id);
            $('#FirstName').val(result[0].firstName);
            $('#LastName').val(result[0].lastName);
            $('#Email').val(result[0].email);
            $('#BirthDate').val(result[0].birthDate);
            $('#PhoneNumber').val(result[0].phoneNumber);
            $('#Address').val(result[0].address);
            $('#DepartmentOption').val(result[0].department_Id);
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
    //debugger;
    var Employee = new Object();
    Employee.Id = $('#Id').val();
    Employee.FirstName = $('#FirstName').val();
    Employee.LastName = $('#LastName').val();
    Employee.Email = $('#Email').val();
    Employee.BirthDate = $('#BirthDate').val();
    Employee.PhoneNumber = $('#PhoneNumber').val();
    Employee.Address = $('#Address').val();
    Employee.Department_Id = $('#DepartmentOption').val();
    $.ajax({
        type: 'POST',
        url: 'Employee/InsertOrUpdate',
        data: Employee
    }).then((result) => {
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Employee Updated Succesfully'
            }).then((result) => {
                if (result.value) {
                    $('#Employee').DataTable().ajax.reload();
                }
            });
        }
        else {
            Swal.fire('Error', 'Failed to Update Employee', 'error');
            $('#myModal').modal('show');
            $('#SaveBtn').hide();
        }
    })
}




function ClearScreen() {
    $('#Id').val('');
    $('#Name').val('');
    $('#EditBtn').hide();
    $('#SaveBtn').show();
}


// Selectlist
function LoadDepartment(element) {
    if (Departments.length === 0) {
        $.ajax({
            type: "Get",
            url: "/Department/LoadDepartment",
            success: function (data) {
                Departments = data.data;
                renderDepartment(element);
            }
        });
    }
    else {
        renderDepartment(element);
    }
}

function renderDepartment(element) {
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('Select Department').hide());
    $.each(Departments, function (i, val) {
        $option.append($('<option/>').val(val.id).text(val.name));
    });
}