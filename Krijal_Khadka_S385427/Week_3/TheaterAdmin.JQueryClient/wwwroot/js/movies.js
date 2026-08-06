const movieApiUrl = `${API_BASE_URL}/movies`;
const movieCategoryApiUrl = `${API_BASE_URL}/categories`;

$(document).ready(function () {
    loadMovieCategories();
    loadMovies();

    $("#movieForm").on("submit", saveMovie);

    $("#cancelMovieEdit").on("click", resetMovieForm);

    $("#refreshMovies").on("click", loadMovies);
});

function loadMovieCategories(selectedId = null) {
    $.ajax({
        url: movieCategoryApiUrl,
        method: "GET"
    })
        .done(function (categories) {
            const dropdown = $("#categoryId");

            dropdown.empty().append(
                $("<option>")
                    .val("")
                    .text("Select category")
            );

            categories.forEach(function (category) {
                dropdown.append(
                    $("<option>")
                        .val(category.id)
                        .text(`${category.name} (${category.code})`)
                );
            });

            if (selectedId !== null) {
                dropdown.val(selectedId);
            }
        })
        .fail(function (xhr) {
            showMovieAlert(
                getMovieErrorMessage(
                    xhr,
                    "Unable to load categories."
                ),
                "danger"
            );
        });
}

function loadMovies() {
    $("#movieTableBody").html(`
        <tr>
            <td colspan="6" class="text-center py-4">
                Loading movies...
            </td>
        </tr>
    `);

    $.ajax({
        url: movieApiUrl,
        method: "GET"
    })
        .done(renderMovies)
        .fail(function (xhr) {
            showMovieAlert(
                getMovieErrorMessage(
                    xhr,
                    "Unable to load movies."
                ),
                "danger"
            );
        });
}

function renderMovies(movies) {
    const tableBody = $("#movieTableBody");
    tableBody.empty();

    if (movies.length === 0) {
        tableBody.html(`
            <tr>
                <td colspan="6" class="text-center py-4 text-secondary">
                    No movies found.
                </td>
            </tr>
        `);

        return;
    }

    movies.forEach(function (movie) {
        const editButton = $("<button>")
            .addClass("btn btn-sm btn-warning me-2")
            .text("Edit")
            .on("click", function () {
                editMovie(movie.id);
            });

        const deleteButton = $("<button>")
            .addClass("btn btn-sm btn-danger")
            .text("Delete")
            .on("click", function () {
                deleteMovie(movie.id, movie.name);
            });

        const actions = $("<td>")
            .addClass("action-buttons")
            .append(editButton, deleteButton);

        const releaseDate = new Date(
            movie.releaseDate
        ).toLocaleDateString();

        const row = $("<tr>").append(
            $("<td>").text(movie.name),
            $("<td>").text(movie.director),
            $("<td>").text(releaseDate),
            $("<td>").text(movie.language),
            $("<td>").text(
                `${movie.categoryName} (${movie.categoryCode})`
            ),
            actions
        );

        tableBody.append(row);
    });
}

function saveMovie(event) {
    event.preventDefault();

    const form = event.currentTarget;

    if (!form.checkValidity()) {
        form.classList.add("was-validated");
        return;
    }

    const movieId = $("#movieId").val();

    const movie = {
        name: $("#movieName").val().trim(),
        director: $("#director").val().trim(),
        email: $("#email").val().trim(),
        releaseDate: $("#releaseDate").val(),
        language: $("#language").val(),
        categoryId: Number($("#categoryId").val())
    };

    const isEditing = movieId !== "";

    $.ajax({
        url: isEditing
            ? `${movieApiUrl}/${movieId}`
            : movieApiUrl,

        method: isEditing ? "PUT" : "POST",

        contentType: "application/json",

        data: JSON.stringify(movie)
    })
        .done(function () {
            showMovieAlert(
                isEditing
                    ? "Movie updated successfully."
                    : "Movie created successfully.",
                "success"
            );

            resetMovieForm();
            loadMovies();
        })
        .fail(function (xhr) {
            showMovieAlert(
                getMovieErrorMessage(
                    xhr,
                    "Unable to save movie."
                ),
                "danger"
            );
        });
}

function editMovie(id) {
    $.ajax({
        url: `${movieApiUrl}/${id}`,
        method: "GET"
    })
        .done(function (movie) {
            $("#movieId").val(movie.id);
            $("#movieName").val(movie.name);
            $("#director").val(movie.director);
            $("#email").val(movie.email);
            $("#releaseDate").val(
                movie.releaseDate.split("T")[0]
            );
            $("#language").val(movie.language);

            loadMovieCategories(movie.categoryId);

            $("#movieFormTitle").text("Edit Movie");
            $("#cancelMovieEdit").removeClass("d-none");

            window.scrollTo({
                top: 0,
                behavior: "smooth"
            });
        })
        .fail(function (xhr) {
            showMovieAlert(
                getMovieErrorMessage(
                    xhr,
                    "Unable to load movie."
                ),
                "danger"
            );
        });
}

function deleteMovie(id, name) {
    const confirmed = confirm(
        `Are you sure you want to delete "${name}"?`
    );

    if (!confirmed) {
        return;
    }

    $.ajax({
        url: `${movieApiUrl}/${id}`,
        method: "DELETE"
    })
        .done(function () {
            showMovieAlert(
                "Movie deleted successfully.",
                "success"
            );

            loadMovies();
        })
        .fail(function (xhr) {
            showMovieAlert(
                getMovieErrorMessage(
                    xhr,
                    "Unable to delete movie."
                ),
                "danger"
            );
        });
}

function resetMovieForm() {
    const form = document.getElementById("movieForm");

    form.reset();
    form.classList.remove("was-validated");

    $("#movieId").val("");
    $("#movieFormTitle").text("Create Movie");
    $("#cancelMovieEdit").addClass("d-none");

    loadMovieCategories();
}

function showMovieAlert(message, type) {
    $("#movieAlert").html(`
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

function getMovieErrorMessage(xhr, defaultMessage) {
    if (xhr.responseJSON?.message) {
        return xhr.responseJSON.message;
    }

    if (xhr.responseJSON?.title) {
        return xhr.responseJSON.title;
    }

    return defaultMessage;
}