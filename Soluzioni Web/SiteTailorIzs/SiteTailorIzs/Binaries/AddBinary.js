// validazione dei controlli in pnlData
function ValidatePnlData()
{

    var result=true;
    var resultp;
    
    //fupFile
    if ($get("fupFile_check").value=="1")
    { 
        resultp = ValidateFupFile();
        result = result && resultp;
    }
    //URL_EXTE
    if ($get("URL_EXTE_check").value=="1")
    { 
        resultp = ValidateURL_EXTE();
        result = result && resultp;
    }
    //ELE_WIDT
    if ($get("ELE_WIDT_check").value=="1")
    { 
        resultp = ValidateELE_WIDT();
        result = result && resultp;
    }
    //ELE_HEIG
    if ($get("ELE_HEIG_check").value=="1") 
    { 
        resultp = ValidateELE_HEIG();
        result = result && resultp;
    } 
    //fupThumbnail
    if ($get("fupThumbnail_check").value=="1")
    { 
        resultp = ValidateFupThumbnail();
        result = result && resultp;
    }

    if(!result) 
    {
        window.alert("Si sono verificati uno o più errori. Controlla i messaggi di errore in rosso.");
        StopEvent();
    }

}

function ValidateFupFile()
{
    var result=false;
    //validazione
    var filename=$get("fupFile").value;
    if (filename=="")
    {
        SetErrLbl("errFupFile","E' necessario selezionare un file");
    }
    else
    {
        //OK , c'è un file
        if (!HasExtension(filename, $get("fupFile_ext").value))
        {
            var extensions=$get("fupFile_ext").value;
            SetErrLbl("errFupFile","E' necessario selezionare un file con una delle seguenti estensioni: " + extensions.substring(0,extensions.length-1));
        }
        else
        {
            SetErrLbl("errFupFile","");
            result=true;
        }
    }
    return result;
}
function ValidateURL_EXTE()
{
    var result=false;
    
    //validazione URL esterno: deve iniziare per http:// o https://
    var URL_EXTE=$get("txtURL_EXTE").value.toLowerCase();
    if (URL_EXTE=="")
    {
        SetYellow($get("txtURL_EXTE"));
        SetErrLbl("errURL_EXTE","E' necessario immettere un indirizzo");
    }
    else
    {
        if (URL_EXTE.substring(0,7)=="http://" || URL_EXTE.substring(0,8)=="https://")
        {
            //OK abbiamo un indirizzo che inizia con...
            if(URL_EXTE.length<13)
            {
                SetYellow($get("txtURL_EXTE"));
                SetErrLbl("errURL_EXTE","L'indirizzo immesso non è valido");
            }
            else
            {
                SetWhite($get("txtURL_EXTE"));
                SetErrLbl("errURL_EXTE","");
                result=true;
            }
        }
        else
        {
            SetYellow($get("txtURL_EXTE"));
            SetErrLbl("errURL_EXTE","L'indirizzo deve iniziare con http:// o https://")
        }    
    }
    
    return result;
}
function ValidateELE_WIDT()
{
    return ValidateWH($get("txtELE_WIDT"),"errELE_WIDT",$get("ELE_WIDT_min").value, $get("ELE_WIDT_max").value); 
}
function ValidateELE_HEIG()
{
    return ValidateWH($get("txtELE_HEIG"),"errELE_HEIG",$get("ELE_HEIG_min").value, $get("ELE_HEIG_max").value); 
}
function ValidateWH(ctl,lblID,min,max)
{
    var result=false;
    
    //validazione larghezza/altezza elemento
    var num=ctl.value;
    if(num=="")
    {
        SetErrLbl(lblID,"E' necessario immettere un valore");
        SetYellow(ctl);
    }
    else
    {
        //ok abbiamo un valore
        if (!isInteger(num))
        {
            SetErrLbl(lblID,"E' necessario immettere un numero intero");
            SetYellow(ctl);
        }
        else
        {
            //ok abbiamo un numero intero
            num=parseInt(num);
            min=parseInt(min);
            max=parseInt(max);
            if(num<=0)
            {
                SetErrLbl(lblID,"E' necessario immettere un numero intero maggiore di zero");
                SetYellow(ctl);
            }
            else
            {
                //ok abbiamo un numero positivo
                if ((min > 0 && (num < min)) || (max > 0 && (num > max)))
                {
                    //non rispetta i vincoli
                    SetErrLbl(lblID,"Il numero immesso non rispetta i limiti imposti");
                    SetYellow(ctl);
                }
                else
                {
                    //ok ci siamo
                    SetErrLbl(lblID, "");
                    SetWhite(ctl);
                    result=true;
                }
            }
        }
    }
    
    return result;
}
function ValidateFupThumbnail()
{
    //validazione file upload thumbnail
    var result=false;
    //validazione
    var filename=$get("fupThumbnail").value;
    if (filename=="")
    {
        //se non c'è un file, allora OK
        SetErrLbl("errFupThumbnail","");
        result=true;
    }
    else
    {
        //OK , c'è un file
        if (!HasExtension(filename, $get("fupThumbnail_ext").value))
        {
            var extensions=$get("fupThumbnail_ext").value;
            SetErrLbl("errFupThumbnail","E' necessario selezionare un file con una delle seguenti estensioni: " + extensions.substring(0,extensions.length-1));
        }
        else
        {
            SetErrLbl("errFupThumbnail","");
            result=true;
        }
    }
    return result;
    
}
function StopEvent(pE)
		{
		if (!pE)
			if (window.event)
			pE = window.event;
			else
			return;
		if (pE.cancelBubble != null)
			pE.cancelBubble = true;
		if (pE.stopPropagation)
			pE.stopPropagation();
		if (pE.preventDefault)
			pE.preventDefault();
		if (window.event)
			pE.returnValue = false;
		if (pE.cancel != null)
			pE.cancel = true;
		}  // StopEvent
		
function SetErrLbl(cid,txt)
{
    $get(cid).innerHTML=txt;
}
function HasExtension(filename,exts)
{
    //determino l'estensione del file
    var temp = new Array();
    temp = filename.split('.');
    var fileext;
    if (temp.length==0)
    {fileext=".notsupported";}
    else
    {fileext="." + temp[temp.length-1].toLowerCase();}

    var extsa = new Array();
    extsa=exts.substring(0,exts.length-1).split(";");
    
    var found=false;
    for(var i=0;i<extsa.length;i++)
    {
        if (fileext==extsa[i].toLowerCase())
        {
            found=true;
            break;
        }
    }
    return(found);
}
function SetYellow(ctl)
{
    ctl.style.backgroundColor="Yellow";
}
function SetWhite(ctl)
{
    ctl.style.backgroundColor="";
}
function isInteger (s)
{
  var i;

  if (isEmpty(s))
  if (isInteger.arguments.length == 1) return 0;
  else return (isInteger.arguments[1] == true);

  for (i = 0; i < s.length; i++)
  {
     var c = s.charAt(i);

     if (!isDigit(c)) return false;
  }

  return true;
}

function isEmpty(s)
{
  return ((s == null) || (s.length == 0))
}

function isDigit (c)
{
  return ((c >= "0") && (c <= "9"))
}
