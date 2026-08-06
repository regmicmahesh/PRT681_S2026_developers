const categoryApiUrl = `${API_BASE_URL}/categories`;

$(document).ready(function () {
    loadCategories();

    $("#categoryForm").on("submit", saveCategory);

    $("#cancelCategoryEdit").on("click", resetCategoryForm);

    $("#refreshCategories").on("click", loadCategories);
});

function loadCategories() {
    $("#categoryTableBody").html(`
        <tr>
            <td colspan="4" class="text-center py-4">
                Loading categories...
            </td>
        </tr>
    `);

    $.ajax({
        url: categoryApiUrl,
        method: "GET"
    })
        .done(renderCategories)
        .fail(function (xhr) {
            showCategoryAlert(
                getErrorMessage(xhr, "Unable to load categories."),
                "danger"
            );
        });
}

function renderCategories(categories) {
    const tableBody = $("#categoryTableBody");
    tableBody.empty();

    if (categories.length === 0) {
        tableBody.html(`
            <tr>
                <td colspan="4" class="text-center py-4 text-secondary">
                    No categories found.
                </td>
            </tr>
        `);

        return;
    }

    categories.forEach(function (category) {
        const editButton = $("<button>")
            .addClass("btn btn-sm btn-warning me-2")
            .text("Edit")
            .on("click", function () {
                editCategory(category.id);
            });

        const deleteButton = $("<button>")
            .addClass("btn btn-sm btn-danger")
            .text("Delete")
            .on("click", function () {
                deleteCategory(category.id, category.name);
            });

        const actions = $("<td>")
            .addClass("action-buttons")
            .append(editButton, deleteButton);

        const row = $("<tr>").append(
            $("<td>").text(category.id),
            $("<td>").text(category.name),
            $("<td>").text(category.code),
            actions
        );

        tableBody.append(row);
    });
}

function saveCategory(event) {
    event.preventDefault();

    const form = event.currentTarget;

    if (!form.checkValidity()) {
        form.classList.add("was-validated");
        return;
    }

    const categoryId = $("#categoryId").val();

    const category = {
        name: $("#categoryName").val().trim(),
        code: $("#categoryCode").val().trim().toUpperCase()
    };

    const isEditing = categoryId !== "";

    $.ajax({
        url: isEditing
            ? `${categoryApiUrl}/${categoryId}`
            : categoryApiUrl,

        method: isEditing ? "PUT" : "POST",

        contentType: "application/json",

        data: JSON.stringify(category)
    })
        .done(function () {
            showCategoryAlert(
                isEditing
                    ? "Category updated successfully."
                    : "Category created successfully.",
                "success"
            );

            resetCategoryForm();
            loadCategories();
        })
        .fail(function (xhr) {
            showCategoryAlert(
                getErrorMessage(xhr, "Unable to save category."),
                "danger"
            );
        });
}

function editCategory(id) {
    $.ajax({
        url: `${categoryApiUrl}/${id}`,
        method: "GET"
    })
        .done(function (category) {
            $("#categoryId").val(category.id);
            $("#categoryName").val(category.name);
            $("#categoryCode").val(category.code);

            $("#categoryFormTitle").text("Edit Category");
            $("#cancelCategoryEdit").removeClass("d-none");

            window.scrollTo({
                top: 0,
                behavior: "smooth"
            });
        })
        .fail(function (xhr) {
            showCategoryAlert(
                getErrorMessage(xhr, "Unable to load category."),
                "danger"
            );
        });
}

function deleteCategory(id, name) {
    const confirmed = confirm(
        `Are you sure you want to delete "${name}"?`
    );

    if (!confirmed) {
        return;
    }

    $.ajax({
        url: `${categoryApiUrl}/${id}`,
        method: "DELETE"
    })
        .done(function () {
            showCategoryAlert(
                "Category deleted successfully.",
                "success"
            );

            loadCategories();
        })
        .fail(function (xhr) {
            showCategoryAlert(
                getErrorMessage(xhr, "Unable to delete category."),
                "danger"
            );
        });
}

function resetCategoryForm() {
    const form = document.getElementById("categoryForm");

    form.reset();
    form.classList.remove("was-validated");

    $("#categoryId").val("");
    $("#categoryFormTitle").text("Create Category");
    $("#cancelCategoryEdit").addClass("d-none");
}

function showCategoryAlert(message, type) {
    $("#categoryAlert").html(`
        <div
            class="alert alert-${type} alert-dismissible fade show"
            role="alert">
            ${message}

            <button
                type="button"
                class="btn-close"
                data-bs-dismiss="alert">
            </button>
        </div>
    `);
}

function getErrorMessage(xhr, defaultMessage) {
    if (xhr.responseJSON?.message) {
        return xhr.responseJSON.message;
    }

    if (xhr.responseJSON?.title) {
        return xhr.responseJSON.title;
    }

    return defaultMessage;
}