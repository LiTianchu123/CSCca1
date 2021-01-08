<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeatherCall.aspx.cs" Inherits="weatherAPI.WeatherCall" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="pageMethods" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <h4>Get weather data demo</h4>
        <br />
        <p>Enter the country or city name</p>
        City/Country:
    <input id="countryName" type="text" />

        <br />
        Enter the number of days you want to see: 
    <input id="numOfdays" type="number" />
        <br />

        <input type="button" id="getWeatherButtonAjax" value="Get Weather by Ajax" />
        <input type="button" id="getWeatherButtonC" value="Get Weather by C#" />


        <table id="result">
        </table>
        <div class="loader"></div>
    </form>
    <style>
        .loader {
            border: 16px solid #f3f3f3; /* Light grey */
            border-top: 16px solid #3498db; /* Blue */
            border-radius: 50%;
            width: 120px;
            height: 120px;
            animation: spin 2s linear infinite;
            display: none;
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>

    <script src="Scripts/jquery-3.4.1.js"></script>

    <script>
        var url = "";
        var apiKey = "**********";
        $('#getWeatherButtonAjax').on('click', function () {
            if (/[^a-zA-Z]/.test($('#countryName').val())) {
                alert("Please enter only alphabets for country/cityname");
            } else {

                $('#result').html('');
                $('.loader').css('display', 'block');
                var countryName = $(`#countryName`).val();
                var numOfdays = $(`#numOfdays`).val();
                url = encodeURI("https://api.worldweatheronline.com/premium/v1/weather.ashx?" + "&key=" + apiKey + "&q=" + countryName + "&format=json&num_of_days=" + numOfdays + "&includeLocation=yes");

                var $tHeader = $(`<thead>Weather in ${countryName} for ${numOfdays} Day(s)</thead>`);
                $('#result').append($tHeader);

                var $tCol = $(`<tr><th>S/N</th><th>Weather at Morning(9am)</th><th>Weather at Night(9pm)</th><th>Highest Temprature/°C</th><th>Lowest Temprature/°C</th><th>Date</th></tr>`);
                $('#result').append($tCol);
                $.ajax({
                    type: 'GET',
                    url: url,
                    dataType: "json",
                }).done(function (data) {
                    console.dir(data);
                    var weatherData = data.data.weather;
                    if (weatherData != null && weatherData != undefined) {
                        var count = 0;
                        for (index = 0; index < weatherData.length; index++) {
                            count++;
                            var maxTempC = weatherData[index].maxtempC;
                            var minTempC = weatherData[index].mintempC;
                            var morningWeather = weatherData[index].hourly[3].weatherDesc[0].value;
                            var nightWeather = weatherData[index].hourly[7].weatherDesc[0].value;
                            var date = weatherData[index].date;
                            var $tRow = $(`<tr><td>${count}</td><td>${morningWeather.toString()}</td><td>${nightWeather.toString()}</td><td>${maxTempC.toString()}</td><td>${minTempC.toString()}</td><td>${date}</td></tr>`);
                            $('#result').append($tRow);
                        }
                    } else {
                        alert(data.data.error[0].msg.toString());
                    }
                    $('.loader').css('display', 'none');
                }).fail(function (data) {
                    console.dir(data);
                    alert("Request failed, Please make sure that you entered correct data.");
                    $('.loader').css('display', 'none');
                });
            }
       

        });

        $('#getWeatherButtonC').on('click', function (e) {
            if (/[^a-zA-Z]/.test($('#countryName').val())) {
                alert("Please enter only alphabets for country/cityname");
            } else {
                e.preventDefault();
                $('#result').html('');
                $('.loader').css('display', 'block');
                var countryName = $(`#countryName`).val();
                var numOfdays = $(`#numOfdays`).val();
                url = encodeURI("https://api.worldweatheronline.com/premium/v1/weather.ashx?" + "&key=" + apiKey + "&q=" + countryName + "&format=json&num_of_days=" + numOfdays + "&includeLocation=yes");

                var $tHeader = $(`<thead>Weather in ${countryName} for ${numOfdays} Day(s)</thead>`);
                $('#result').append($tHeader);

                var $tCol = $(`<tr><th>S/N</th><th>Weather at Morning(9am)</th><th>Weather at Night(9pm)</th><th>Highest Temprature/°C</th><th>Lowest Temprature/°C</th><th>Date</th></tr>`);
                $('#result').append($tCol);

                console.dir(PageMethods.get_path());
                PageMethods.set_path("WeatherCall" + '.aspx');
                PageMethods.GetWeather(countryName, numOfdays, OnSucceeded, OnFailed);

                function OnSucceeded(result) {
                   
                    var count = 0;
                    var weatherData = JSON.parse(result).weather;
                    if (weatherData != undefined && weatherData != null) {
                        for (index = 0; index < weatherData.length; index++) {
                            count++;
                            var maxTempC = weatherData[index].maxtempC;
                            var minTempC = weatherData[index].mintempC;
                            var morningWeather = weatherData[index].hourly[3].weatherDesc[0].value;
                            var nightWeather = weatherData[index].hourly[7].weatherDesc[0].value;
                            var date = weatherData[index].date;
                            var $tRow = $(`<tr><td>${count}</td><td>${morningWeather.toString()}</td><td>${nightWeather.toString()}</td><td>${maxTempC.toString()}</td><td>${minTempC.toString()}</td><td>${date}</td></tr>`);
                            $('#result').append($tRow);
                        }
                    } else {
                        console.dir(JSON.parse(result));
                        alert(JSON.parse(result).error[0].msg.toString());
                    }
                    $('.loader').css('display', 'none');
                }


                function OnFailed(error) {
                    $('.loader').css('display', 'none');
                    alert("Not able to get the weather, check your country/city name, or try again with lesser number of days(max 5)");
                }
            }

        });


    </script>

</body>
</html>
