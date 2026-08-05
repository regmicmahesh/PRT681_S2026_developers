const apiBaseUrl = "http://localhost:5030/api";

let categoryModal;

$(function () {
  categoryModal = new bootstrap.Modal(document.getElementById('categoryModal'));
  loadCategories();

  $('#categoryForm').on('submit', function (e) {
    e.preventDefault();
    saveCategory();
  });
});

function loadCategories() {
  $.ajax({
    url: `${apiBaseUrl}/Category`,
    method: 'GET',
    dataType: 'json'
  }).done(function (categories) {
    const rows = categories.map(c => `
      <tr>
        <td>${c.categoryId}</td>
        <td>${c.categoryName}</td>
        <td>${c.categoryCode}</td>
        <td>
          <button class="btn btn-sm btn-outline-primary" onclick="openEditCategory(${c.categoryId})">Edit</button>
          <button class="btn btn-sm btn-outline-danger" onclick="deleteCategory(${c.categoryId})">Delete</button>
        </td>
      </tr>`).join('');
    $('#categoryTableBody').html(rows);
  }).fail(function (xhr) {
    showCategoryError(`Failed to load categories: ${xhr.status} ${xhr.statusText}`);
  });
}

function openCreateCategory() {
  $('#categoryModalTitle').text('New Category');
  $('#categoryId').val('');
  $('#categoryForm')[0].reset();
}

function openEditCategory(id) {
  $.ajax({ url: `${apiBaseUrl}/Category/${id}`, method: 'GET' })
    .done(function (c) {
      $('#categoryModalTitle').text('Edit Category');
      $('#categoryId').val(c.categoryId);
      $('#categoryName').val(c.categoryName);
      $('#categoryCode').val(c.categoryCode);
      categoryModal.show();
    });
}

function saveCategory() {
  const id = $('#categoryId').val();
  const payload = {
    categoryName: $('#categoryName').val(),
    categoryCode: $('#categoryCode').val()
  };

  const isEdit = !!id;
  if (isEdit) payload.categoryId = parseInt(id);

  $.ajax({
    url: isEdit ? `${apiBaseUrl}/Category/${id}` : `${apiBaseUrl}/Category`,
    method: isEdit ? 'PUT' : 'POST',
    contentType: 'application/json',
    data: JSON.stringify(payload)
  }).done(function () {
    categoryModal.hide();
    $('#categoryAlert').addClass('d-none');
    loadCategories();
  }).fail(function (xhr) {
    showCategoryError(formatValidationErrors(xhr));
  });
}

function deleteCategory(id) {
  if (!confirm('Delete this category?')) return;

  $.ajax({ url: `${apiBaseUrl}/Category/${id}`, method: 'DELETE' })
    .done(function () { loadCategories(); })
    .fail(function (xhr) {
      // A 500/conflict here usually means movies still reference this category
      showCategoryError('Could not delete category — it may still be assigned to a movie.');
    });
}

function showCategoryError(message) {
  $('#categoryAlert').removeClass('d-none').text(message);
}

// ASP.NET Core returns ValidationProblemDetails { errors: { Field: ["message"] } } on 400
function formatValidationErrors(xhr) {
  if (xhr.status === 400 && xhr.responseJSON && xhr.responseJSON.errors) {
    return Object.values(xhr.responseJSON.errors).flat().join(' ');
  }
  return `Save failed: ${xhr.status} ${xhr.statusText}`;
}
