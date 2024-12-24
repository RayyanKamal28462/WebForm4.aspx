<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="WebForm4.aspx.cs" Inherits="InternWebApplication.WebForm4" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Styling for the Prayer Times Form */
        .prayer-times-container {
            max-width: 500px;
            margin: 0 auto;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            background-color: #f9f9f9;
        }

        .prayer-times-container h2 {
            text-align: center;
            color: #2c3e50;
        }

        .prayer-times-container label {
            font-weight: bold;
            margin-bottom: 8px;
            color: #34495e;
            display: inline-block;
        }

        .prayer-times-container input[type="text"] {
            width: 100%;
            padding: 8px;
            margin-bottom: 15px;
            border: 1px solid #ccc;
            border-radius: 4px;
            font-size: 14px;
        }

        .prayer-times-container button {
            width: 100%;
            padding: 10px;
            background-color: #2980b9;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            transition: background-color 0.3s ease;
        }

        .prayer-times-container button:hover {
            background-color: #3498db;
        }

        .prayer-times-container .result-label {
            margin-top: 20px;
            text-align: center;
            display: block;
            color: #2c3e50;
            font-size: 16px;
        }

        /* Add some responsive design */
        @media (max-width: 600px) {
            .prayer-times-container {
                padding: 15px;
            }

            .prayer-times-container input[type="text"] {
                font-size: 16px;
            }

            .prayer-times-container button {
                font-size: 18px;
            }
        }
    </style>
         <script type="text/javascript">
             // Function to get the user's location using the Geolocation API
             function getLocation() {
                 if (navigator.geolocation) {
                     navigator.geolocation.getCurrentPosition(function (position) {
                         // Get latitude and longitude from the position object
                         var latitude = position.coords.latitude;
                         var longitude = position.coords.longitude;

                         // Set the latitude and longitude in the input fields
                         document.getElementById('<%= txtLatitude.ClientID %>').value = latitude;
                    document.getElementById('<%= txtLongitude.ClientID %>').value = longitude;
                }, function (error) {
                    // Handle error if location access is denied or unavailable
                    alert("Error occurred while fetching your location. Please enter coordinates manually.");
                });
                 } else {
                     alert("Geolocation is not supported by this browser.");
                 }
             }

             // Call getLocation() when the page loads
             window.onload = function () {
                 getLocation();
             };
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="prayer-times-container">
        <h2>Prayer Times</h2>

        <label for="txtLatitude">Latitude:</label>
        <input type="text" id="txtLatitude" runat="server" readonly /><br />

        <label for="txtLongitude">Longitude:</label>
        <input type="text" id="txtLongitude" runat="server" readonly /><br />

        <button type="button" id="btnGetPrayerTimes" runat="server" onserverclick="btnGetPrayerTimes_Click">Get Prayer Times</button><br />

        <asp:Label ID="lblPrayerTimes" runat="server" CssClass="result-label" Text=""></asp:Label>
    </div>
</asp:Content>
