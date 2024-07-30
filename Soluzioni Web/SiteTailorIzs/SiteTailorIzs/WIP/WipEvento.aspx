<%@ Page Title="" Language="vb" AutoEventWireup="false" CodeBehind="WipEvento.aspx.vb" Inherits="Softailor.SiteTailorIzs.WipEvento" %>
<!DOCTYPE html>
<html lang="it-it">

<head id="Head1" runat="server">
    <title>Scheda Programmazione Evento</title>
    <link href="//fonts.googleapis.com/css?family=Oswald:400,300" rel="stylesheet" type="text/css">
    <link href="WipEvento.css" rel="stylesheet" />
    <link rel="stylesheet" href="../Scripts/DatePicker/css/softailor.css" type="text/css">
    <script src="../Scripts/jquery-1.7.1.min.js"></script>
    <script src="../Scripts/date-it-IT.js"></script>
    <script src="../Scripts/jquery.maskedinput.min.js"></script>
    <script src="../Scripts/jquery.validate.min.js"></script>
    <script src="../Scripts/messages_it.js"></script>
    <script src="WipEvento.js"></script>
    <script type="text/javascript" src="../Scripts/DatePicker/javascript/zebra_datepicker.js"></script>
    <style type="text/css">
        body
        {
            font-family:Arial;
            font-size:12px;
            background-color:#ffffff;
            margin:0px;
            padding:0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:PlaceHolder ID="phdContent" runat="server" EnableViewState="false" />
        <div id="valueStringPopup" class="popup">
        <div id="labelString" class="labelDiv"></div>
        <div class="valueDiv">
            <input type="text" id="valueString" name="valueString" class="inputtext inputstring" maxlength="400" />
        </div>
        <div class="buttonsDiv">
            <span class="save" id="saveString">Salva</span>
            <span class="cancel">Annulla</span>
        </div>
        <div class="updatedDiv" id="updatedString"></div>
    </div>
    <div id="valueIntPopup" class="popup">
        <div id="labelInt" class="labelDiv"></div>
        <div class="valueDiv">
            <input type="text" id="valueInt" class="inputtext inputint" name="valueInt" />
        </div>
        <div class="buttonsDiv">
            <span class="save" id="saveInt">Salva</span>
            <span class="cancel">Annulla</span>
        </div>
        <div class="updatedDiv" id="updatedInt"></div>
    </div>
    <div id="valueDatePopup" class="popup">
        <div id="labelDate" class="labelDiv"></div>
        <div class="valueDiv">
            <input type="text" id="valueDate" class="inputtext inputdate" name="valueDate" />
            <div class="calcontainer">
                <div id="valueDateCalendar"></div>
            </div>
        </div>
        <div class="buttonsDiv">
            <span class="save" id="saveDate">Salva</span>
            <span class="cancel">Annulla</span>
        </div>
        <div class="updatedDiv" id="updatedDate"></div>
    </div>
    <div id="valueBooleanPopup" class="popup">
        <div id="labelBoolean" class="labelDiv"></div>
        <div class="valueDiv">
            <input type="checkbox" id="valueBoolean" class="inputboolean" name="valueBoolean" />
        </div>
        <div class="buttonsDiv">
            <span class="save" id="saveBoolean">Salva</span>
            <span class="cancel">Annulla</span>
        </div>
        <div class="updatedDiv" id="updatedBoolean"></div>
    </div>
    <div id="valueMoneyPopup" class="popup">
        <div id="labelMoney" class="labelDiv"></div>
        <div class="valueDiv">
            <input type="text" id="valueMoney" class="inputtext inputmoney" name="valueMoney" />
        </div>
        <div class="buttonsDiv">
            <span class="save" id="saveMoney">Salva</span>
            <span class="cancel">Annulla</span>
        </div>
        <div class="updatedDiv" id="updatedMoney"></div>
    </div>
    <div id="wipTBCPopup" class="popup">
        <div id="TBCTitle2" class="wipTitle2"></div>
        <div id="TBCTitle" class="wipTitle"></div>
        <div class="wipExpiry">
            Scadenza: <b><span id="TBCExpiry"></span></b>
        </div>
        <div class="wipButtonsMain">
            <div id="TBC_OK" class="wipBtn wipBtnOK">Completato</div>
            <div id="TBC_NN" class="wipBtn wipBtnNN">Non Necessario</div>
        </div>
        <div class="wipExpiryDiv">
            Scadenza:
            <input type="text" name="TBC_NewExpiry" id="TBC_NewExpiry" class="wipExpiryDate" />
            <div id="TBC_NEWSCAD" class="wipBtn wipBtnModScad">
                Modifica
            </div>
        </div>
        <div>
            <div id="TBC_Calendar" class="calcontainerspt"></div>
        </div>
        <div>
        </div>
        <div class="wipButtonsEnd">
            <span class="cancel">Annulla</span>
        </div>
    </div>
    <div id="wipOKPopup" class="popup">
        <div id="OKTitle2" class="wipTitle2"></div>
        <div id="OKTitle" class="wipTitle"></div>
        <div class="wipExpiry">
            Completato il 
            <b><span id="OKCompletedOn"></span></b>
            da
            <b><span id="OKCompletedBy"></span></b>
        </div>
        <div class="wipButtonsMain">
            <div id="OK_TBC" class="wipBtn wipBtnTBC">Da Eseguire</div>
            <div id="OK_NN" class="wipBtn wipBtnNN">Non Necessario</div>
        </div>
        <div class="wipButtonsEnd">
            <span class="cancel">Annulla</span>
        </div>
    </div>
    <div id="wipKOPopup" class="popup">
        <div id="KOTitle2" class="wipTitle2"></div>
        <div id="KOTitle" class="wipTitle"></div>
        <div class="wipExpiry" style="color:red;">
            Scaduto il <b><span id="KOExpiry"></span></b>
        </div>
        <div class="wipButtonsMain">
            <div id="KO_OK" class="wipBtn wipBtnOK">Completato</div>
            <div id="KO_NN" class="wipBtn wipBtnNN">Non Necessario</div>
        </div>
        <div class="wipExpiryDiv">
            Scadenza:
            <input type="text" name="KO_NewExpiry" id="KO_NewExpiry" class="wipExpiryDate" />
            <div id="KO_NEWSCAD" class="wipBtn wipBtnModScad">
                Modifica
            </div>
        </div>
        <div>
            <div id="KO_Calendar" class="calcontainerspt"></div>
        </div>
        <div>
        </div>
        <div class="wipButtonsEnd">
            <span class="cancel">Annulla</span>
        </div>
    </div>
    <div id="wipNNPopup" class="popup">
        <div id="NNTitle2" class="wipTitle2"></div>
        <div id="NNTitle" class="wipTitle"></div>
        <div class="wipExpiry">
            Contrassegnato come <b>non necessario</b>
            il
            <b><span id="NNCompletedOn"></span></b>
            da
            <b><span id="NNCompletedBy"></span></b>
        </div>
        <div class="wipButtonsMain">
            <div id="NN_TBC" class="wipBtn wipBtnTBC">Da Eseguire</div>
            <div id="NN_OK" class="wipBtn wipBtnOK">Completato</div>
        </div>
        <div class="wipButtonsEnd">
            <span class="cancel">Annulla</span>
        </div>
    </div>

    </form>
</body>
</html>