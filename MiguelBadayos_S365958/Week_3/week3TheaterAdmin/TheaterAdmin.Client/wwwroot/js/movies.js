const apiBaseUrl = "http://localhost:5030/api";

let movieModal;

$(function () {
  movieModal = new bootstrap.Modal(document.getElementById('movieModal'));
  loadCategoryDropdown();
  loadMovies();

  $('#movieForm').on('submit', function (e) {
    e.preventDefault();
    saveMovie();
  });
});

function loadCategoryDropdown() {
  $.ajax({ url: `${apiBaseUrl}/Category`, method: 'GET' })
    .done(function (categories) {
      const options = categories.map(c => `<option value="${c.categoryId}">${c.categoryName}</option>`).join('');
      $('#movieCategoryId').html(options);
    });
}

function loadMovies() {
  $.ajax({ url: `${apiBaseUrl}/Movie`, method: 'GET' })
    .done(function (movies) {
      const rows = movies.map(m => `
        <tr>
          <td>${m.movieId}</td>
          <td>${m.movieName}</td>
          <td>${m.releaseDate}</td>
          <td>${m.director}</td>
          <td>${m.contactEmailAddress}</td>
          <td>${m.language}</td>
          <td>${m.category ? m.category.categoryName : ''}</td>
          <td>
            <button class="btn btn-sm btn-outline-primary" onclick="openEditMovie(${m.movieId})">Edit</button>
            <button class="btn btn-sm btn-outline-danger" onclick="deleteMovie(${m.movieId})">Delete</button>
          </td>
        </tr>`).join('');
      $('#movieTableBody').html(rows);
    }).fail(function (xhr) {
      showMovieError(`Failed to load movies: ${xhr.status} ${xhr.statusText}`);
    });
}

function openCreateMovie() {
  $('#movieModalTitle').text('New Movie');
  $('#movieForm')[0].reset();
  $('#movieId').val('');
}

function openEditMovie(id) {
  $.ajax({ url: `${apiBaseUrl}/Movie/${id}`, method: 'GET' })
    .done(function (m) {
      $('#movieModalTitle').text('Edit Movie');
      $('#movieId').val(m.movieId);
      $('#movieName').val(m.movieName);
      $('#movieReleaseDate').val(m.releaseDate.substring(0, 10)); // yyyy-MM-dd for <input type=date>
      $('#movieDirector').val(m.director);
      $('#movieEmail').val(m.contactEmailAddress);
      $('#movieLanguage').val(m.language);
      $('#movieCategoryId').val(m.categoryId);
      movieModal.show();
    });
}

function saveMovie() {
  const id = $('#movieId').val();
  const isEdit = !!id;

  const payload = {
    movieName: $('#movieName').val(),
    releaseDate: $('#movieReleaseDate').val(),
    director: $('#movieDirector').val(),
    contactEmailAddress: $('#movieEmail').val(),
    language: $('#movieLanguage').val(),
    categoryId: parseInt($('#movieCategoryId').val())
  };
  if (isEdit) payload.movieId = parseInt(id);

  $.ajax({
    url: isEdit ? `${apiBaseUrl}/Movie/${id}` : `${apiBaseUrl}/Movie`,
    method: isEdit ? 'PUT' : 'POST',
    contentType: 'application/json',
    data: JSON.stringify(payload)
  }).done(function () {
    movieModal.hide();
    $('#movieAlert').addClass('d-none');
    loadMovies();
  }).fail(function (xhr) {
    showMovieError(formatValidationErrors(xhr));
  });
}

function deleteMovie(id) {
  if (!confirm('Delete this movie?')) return;

  $.ajax({ url: `${apiBaseUrl}/Movie/${id}`, method: 'DELETE' })
    .done(function () { loadMovies(); })
    .fail(function (xhr) { showMovieError(`Delete failed: ${xhr.status} ${xhr.statusText}`); });
}

function showMovieError(message) {
  $('#movieAlert').removeClass('d-none').text(message);
}

function formatValidationErrors(xhr) {
  if (xhr.status === 400 && xhr.responseJSON && xhr.responseJSON.errors) {
    return Object.values(xhr.responseJSON.errors).flat().join(' ');
  }
  return `Save failed: ${xhr.status} ${xhr.statusText}`;
}
