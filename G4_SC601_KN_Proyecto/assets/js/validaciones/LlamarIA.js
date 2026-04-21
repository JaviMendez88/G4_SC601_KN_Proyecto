@section Scripts {
    <script>
        function llamarIA() {
    var msg = $('#userInput').val();
        if(msg.trim() === "") return;

        $('#chatWindow').append('<div class="text-right mb-2"><strong>Tú:</strong> ' + msg + '</div>');
        $('#userInput').val('');

        $.post('@Url.Action("Consultar", "Gemini")', {pregunta: msg }, function(data) {
            $('#chatWindow').append('<div class="text-left mb-2 text-primary"><strong>Gemini:</strong> ' + data.respuesta + '</div>');
    });
}
    </script>
}