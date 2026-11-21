// Function to load comments
function loadComments(projectId) {
    $.ajax({
        url: '/ProjectManagement/ProjectComment/GetComments?projectId=' + projectId,
        method: 'GET',
        success: function (data) {
            var commentsHtml = '';
            for (var i = 0; i < data.length; i++) {
                commentsHtml += '<div class="comment">';
                commentsHtml += '<p>' + data[i].content + '</p>';
                commentsHtml += '<span><b>Posted on:</b><i> ' + new Date(data[i].datePosted).toLocaleString() + '<i></span>';
                commentsHtml += '</div>';
            }
            $('#commentsList').html(commentsHtml);
        }
    });
}

$(document).ready(function () {
    var projectId = $('#projectComments input[name="ProjectId"]').val();

    // Call loadComments on page load
    loadComments(projectId);

    // Submit event for addCommentForm
    $('#addCommentForm').submit(function (e) {
        e.preventDefault();
        var formData = {
            ProjectId: projectId,
            Content: $('#projectComments textarea[name="Content"]').val()
        };

        $.ajax({
            url: '/ProjectManagement/ProjectComment/AddComment',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),       //convert to JSON string
            success: function (response) {
                if (response.success) {
                    $('#projectComments textarea[name="Content"]').val(''); // Clear the textarea
                    loadComments(projectId); // Reload comments after adding new one
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr, status, error) {
                alert("Error: " + error);
            }
        });
    });

});
