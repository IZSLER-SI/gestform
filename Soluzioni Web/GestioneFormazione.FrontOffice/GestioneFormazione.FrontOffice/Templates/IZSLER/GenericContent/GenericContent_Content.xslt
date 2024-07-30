<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
  <xsl:param name="gcontentkey" />
  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="$gcontentkey='notelegali'">
        <xsl:call-template name="notelegali" />
      </xsl:when>
      <xsl:when test="$gcontentkey='privacy'">
        <xsl:call-template name="privacy" />
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="notelegali">
    <div class="title green">Note Legali</div>
    <div>
      L'istituto Zooprofilattico della Lombardia e dell'Emilia Romagna (di seguito "IZSLER") ha realizzato il sito per 
      favorire la comunicazione e l'erogazione di servizi  verso le amministrazioni, le aziende e i cittadini destinatari 
      della propria attività istituzionale.<br />
      <br />
      <strong>ACCESSIBILITA'</strong><br />
      Particolare attenzione è stata osservata, nelle fasi di progettazione e di realizzazione del sito, riguardo alle 
      caratteristiche di accessibilità e di usabilità. Salvo errori o omissioni sui quali IZSLER interverrà prontamente, 
      è stata posta la massima cura affinché le pagine siano conformi ai requisiti tecnici indicati nel Decreto 
      Ministeriale di attuazione della Legge Stanca (Legge 4/2004).
      <br />
      <br />
      IZSLER sollecita ogni segnalazione o suggerimento utile al miglioramento del grado di accessibilità del sito. 
      Eventuali comunicazioni a tale riguardo possono essere inoltrate a 
      <a href="mailto:webmaster@izs.bs.it">webmaster@izs.bs.it</a>
      <br />
      <br />
      <strong>FRUIZIONE</strong><br />
      Sono state condotte prove approfondite per verificare la compatibilità di diversi browser, in differenti versioni.<br />
      Per quanto è stato possibile accertare, le informazioni del sito sono visualizzabili correttamente - 
      seppure non identicamente, dal punto di vista puramente grafico - con i browser di seguito elencati, 
      nelle versioni rispettivamente indicate o successive:<br /><br />
      <strong>Sistemi Windows</strong><br />
      Internet Explorer 9 o superiore<br />
      Firefox 15 o superiore<br />
      Google Chrome 23 o superiore<br /><br />
      <strong>Sistemi Mac</strong><br />
      Safari 5 o superiore<br /><br />
      I documenti sono pubblicati prevalentemente in file formato PDF. Per la consultazione è necessario il software Adobe 
      Acrobat Reader, disponibile gratuitamente in molte lingue (anche l'italiano) e per molte piattoforme tra cui Windows, 
      Macintosh e Unix/Linux.<br />Sono anche presenti file compressi in formato ZIP e file in formato DOC e RTF.<br />
      Nella sezione Formazione, la fruizione dei corsi e la consultazione di altre informazioni richiedono software 
      specifico, indicato di volta in volta.<br /><br />
      <strong>RESPONSABILITA'</strong><br />
      IZSLER, salvo diverse indicazioni di legge, non potrà essere ritenuto in alcun modo responsabile dei danni di qualsiasi 
      natura causati direttamente o indirettamente dall'accesso al sito, dall'incapacità o impossibilità di accedervi, 
      dall'affidamento alle notizie in esso contenute o dal loro impiego.<br /><br />IZSLER provvede ad inserire nel sito 
      informazioni per quanto possibile aggiornate, ma non garantisce circa la loro completezza o accuratezza. In particolare
      - per quanto riguarda i testi della normativa, di avvisi, di bandi di gara o di concorso - il Cnipa fornisce a solo 
      scopo divulgativo tali informazioni, che non costituiscono perciò fonte di diritto. Si raccomanda quindi, laddove 
      appropriato, di consultare le fonti ufficiali (Gazzette Ufficiali).<br />IZSLER si riserva il diritto di modificare 
      i contenuti del sito e di questa Guida in qualsiasi momento e senza alcun preavviso.<br /><br />
      <strong>SITI ESTERNI COLLEGATI</strong><br />
      I collegamenti a siti esterni sono forniti come semplice servizio agli utenti, con esclusione di ogni responsabilità 
      sulla correttezza e sulla completezza dell'insieme dei collegamenti indicati.<br /><br />L'indicazione dei collegamenti 
      non implica da parte di IZSLER alcun tipo di approvazione o condivisione di responsabilità in relazione alla 
      completezza e alla correttezza delle informazioni contenute nei siti indicati.<br /><br />
      <strong>COPYRIGHT</strong><br />
      I contenuti del sito - codice di script, grafica, testi, tabelle, immagini, suoni, e ogni altra informazione 
      disponibile in qualunque forma - sono protetti ai sensi della normativa in tema di opere dell'ingegno.<br /><br />
      Tutte le aziende e i prodotti menzionati in questo sito sono identificati dai rispettivi marchi che sono o possono 
      essere protetti da brevetti e/o copyright concessi o registrati dalle autorità preposte. I prodotti software e i 
      contenuti informativi, salvo diverse specifiche indicazioni, possono essere scaricati o utilizzati solo per uso 
      personale, o comunque non commerciale citando la fonte. <br /><br />Per fini di lucro è consentito utilizzare, 
      copiare e distribuire i documenti e le relative immagini disponibili su questo sito solo dietro permesso scritto 
      (o egualmente valido a fini legali) di IZSLER, fatte salve eventuali spettanze di diritto. Le note di copyright, 
      gli autori ove indicati o la fonte stessa devono in tutti i casi essere citati nelle pubblicazioni in qualunque 
      forma realizzate e diffuse.
    </div>
  </xsl:template>

  <xsl:template name="privacy">
    <div class="title green">La Privacy Policy di questo sito</div>
    <strong>PERCHE' QUESTO AVVISO</strong>

    <p>In questa pagina si descrivono le modalità di gestione del sito in riferimento al trattamento dei dati personali degli utenti che lo consultano.</p>
    <p>
      Si tratta di un'informativa che è resa anche ai sensi dell'art. 13 del d.lgs. n. 196/2003 - Codice in materia di protezione dei dati personali  a coloro che 
      interagiscono con i servizi web dell' Istituto Zooprofilattico Sperimentale della Lombardia ed Emilia Romagna (da qui in poi: IZSLER), 
      accessibili per via telematica a partire dall'indirizzo: <a href="http://www.izsler.it/">http://www.izsler.it</a> corrispondente alla 
      pagina iniziale del sito ufficiale dell'IZSLER.
    </p>
    <p>L'informativa è resa solo per il sito dell'IZSLER e non anche per altri siti web eventualmente consultati dall'utente tramite link.</p>
    <p>L'informativa si ispira anche alla Raccomandazione n. 2/2001 che le autorità europee per la protezione dei dati personali, riunite nel 
    Gruppo istituito dall'art. 29 della direttiva n. 95/46/CE, hanno adottato il 17 maggio 2001 per individuare alcuni requisiti minimi per la 
    raccolta di dati personali on-line, e, in particolare, le modalità, i tempi e la natura delle informazioni che i titolari del trattamento 
    devono fornire agli utenti quando questi si collegano a pagine web, indipendentemente dagli scopi del collegamento.</p>
    <p>
      <br />
      <strong>IL "TITOLARE" DEL TRATTAMENTO</strong>
    </p>
    <p>
      A seguito della consultazione di questo sito possono essere trattati dati relativi a persone identificate o identificabili.<br />Il "titolare" 
      del loro trattamento è l'Istituto Zooprofilattico Sperimentale della Lombardia ed Emilia ROmagna, che ha sede in Brescia (Italia), Via Bianchi 9, CAP 25124.
    </p>
    <p>
      <br />
      <strong>LUOGO DI TRATTAMENTO DEI DATI</strong>
    </p>
    <p>
      I trattamenti connessi ai servizi web di questo sito hanno luogo presso la predetta sede dell'IZSELR e sono curati solo da personale tecnico dell'Ufficio 
      incaricato del trattamento, oppure da eventuali incaricati di occasionali operazioni di manutenzione.<br />Nessun dato derivante dal servizio web 
      viene comunicato o diffuso.<br />I dati personali forniti dagli utenti che inoltrano richieste di invio di materiale informativo sono utilizzati 
      al solo fine di eseguire il servizio o la prestazione richiesta e sono comunicati a terzi nel solo caso in cui ciò sia a tal fine necessario.<br /><br /><br />
      <strong>TIPI DI DATI TRATTATI</strong>
    </p>
    <p>Dati di navigazione</p>
    <p>
      I sistemi informatici e le procedure software preposte al funzionamento di questo sito web acquisiscono, nel corso del loro normale esercizio, alcuni dati 
      personali la cui trasmissione è implicita nell'uso dei protocolli di comunicazione di Internet.<br />Si tratta di informazioni che non sono raccolte per 
      essere associate a interessati identificati, ma che per loro stessa natura potrebbero, attraverso elaborazioni ed associazioni con dati detenuti da terzi, 
      permettere di identificare gli utenti.<br />In questa categoria di dati rientrano gli indirizzi IP o i nomi a dominio dei computer utilizzati dagli utenti 
      che si connettono al sito, gli indirizzi in notazione URI (Uniform Resource Identifier) delle risorse richieste, l'orario della richiesta, il metodo utilizzato 
      nel sottoporre la richiesta al server, la dimensione del file ottenuto in risposta, il codice numerico indicante lo stato della risposta data dal server 
      (buon fine, errore, ecc.) ed altri parametri relativi al sistema operativo e all'ambiente informatico dell'utente.<br />Questi dati vengono utilizzati al 
      solo fine di ricavare informazioni statistiche anonime sull'uso del sito e per controllarne il corretto funzionamento e vengono cancellati immediatamente 
      dopo l'elaborazione. I dati potrebbero essere utilizzati per l'accertamento di responsabilità in caso di ipotetici reati informatici ai danni del sito.
    </p>
    <p>
      <br />Dati forniti volontariamente dall'utente
    </p>
    <p>
      L'invio facoltativo, esplicito e volontario di posta elettronica agli indirizzi indicati su questo sito comporta la successiva acquisizione 
      dell'indirizzo del mittente, necessario per rispondere alle richieste, nonché degli eventuali altri dati personali inseriti nella missiva.<br />
      Specifiche informative di sintesi verranno progressivamente riportate o visualizzate nelle pagine del sito predisposte per particolari servizi a richiesta.
    </p>
    <p>
      <br />
      <strong>COOKIES</strong>
    </p>
    <p>
      Nessun dato personale degli utenti viene in proposito acquisito dal sito.<br />Non viene fatto uso di cookies per la trasmissione di informazioni di 
      carattere personale, né vengono utilizzati c.d. cookies persistenti di alcun tipo, ovvero sistemi per il tracciamento degli utenti.<br />L'uso di c.d. 
      cookies di sessione (che non vengono memorizzati in modo persistente sul computer dell'utente e svaniscono con la chiusura del browser) è strettamente 
      limitato alla trasmissione di identificativi di sessione (costituiti da numeri casuali generati dal server) necessari per consentire l'esplorazione 
      sicura ed efficiente del sito.<br />I c.d. cookies di sessione utilizzati in questo sito evitano il ricorso ad altre tecniche informatiche 
      potenzialmente pregiudizievoli per la riservatezza della navigazione degli utenti e non consentono l'acquisizione di dati personali identificativi dell'utente.
    </p>
    <p>
      <br />
      <strong>FACOLTATIVITA' DEL CONFERIMENTO DEI DATI</strong>
    </p>
    <p>
      A parte quanto specificato per i dati di navigazione, l'utente è libero di fornire i dati personali riportati nei moduli di richiesta per sollecitare 
      l'invio di materiale informativo o di altre comunicazioni.<br />Il loro mancato conferimento può comportare l'impossibilità di ottenere quanto richiesto.
    </p>
    <p>
      <br />
      <strong>MODALITA' DEL TRATTAMENTO</strong>
    </p>
    <p>
      I dati personali sono trattati con strumenti automatizzati per il tempo strettamente necessario a conseguire gli scopi per cui sono stati raccolti.<br />
      Specifiche misure di sicurezza sono osservate per prevenire la perdita dei dati, usi illeciti o non corretti ed accessi non autorizzati.
    </p>
    <p>
      <br />
      <strong>DIRITTI DEGLI INTERESSATI</strong>
    </p>
    <p>
      I soggetti cui si riferiscono i dati personali hanno il diritto in qualunque momento di ottenere la conferma dell'esistenza o meno dei 
      medesimi dati e di conoscerne il contenuto e l'origine, verificarne l'esattezza o chiederne l'integrazione o l'aggiornamento, oppure la rettificazione (art. 7 del d.lgs. n. 196/2003).<br />Ai sensi del medesimo articolo si ha il diritto di chiedere la cancellazione, la trasformazione in forma anonima o il blocco dei dati trattati in violazione di legge, nonché di opporsi in ogni caso, per motivi legittimi, al loro trattamento.<br />Le richieste possono essere rivolte all'URP (<a href="mailto:urp@izsler.it">urp@izsler.it</a>).<br /> 
    </p>
    <p>
      <a target="_blank" href="http://www.izsler.it/izs_bs/allegati/210/Cod14a_Informativa_clienti.pdf">INFORMATIVA PER CLIENTI</a> (.pdf 46KB)
    </p>
    <p>
      <a target="_blank" href="http://www.izsler.it/izs_bs/allegati/210/Cod14b_Informativa_fornitori.pdf">INFORMATIVA PER FORNITORI</a> (.pdf 36KB)
    </p>
    <p>
      <a target="_blank" href="http://www.izsler.it/izs_bs/allegati/210/INFORMATIVA_PRIVACY_FORMAZIONE.pdf">Informativa Privacy per partecipanti a corsi/convegni organizzati dall'IZSLER</a> (.pdf 13 KB)
    </p>
    <p>
      RESPONSABILI DEL TRATTAMENTO<br />I responsabili dei trattamenti dei dati sono stati nominati con apposita delibera del Direttore Generale
    </p>
    <p>
      <a target="_blank" href="http://www.izsler.it/izs_bs/allegati/210/Risposta_richieste_consenso.pdf">RISPOSTA A RICHIESTE DI CONSENSO AL TRATTAMENTO NOSTRI DATI</a> (.pdf 31 KB)
    </p>
    <p>
      Regolamento per il trattamento dei dati sensibili<br />
      Bollettino Ufficiale Regione Lombardia - 1° supplemento ordinario al n.17 - 27 Aprile 2006<br />
      Bollettino Ufficiale Regione Lombardia - 2° supplemento ordinario al n.29 - 21 Luglio 2006<br />
      Bollettino Ufficiale Regione Emilia Romagna -  n.57 - 24 Aprile 2006
    </p>
  </xsl:template>


</xsl:stylesheet>
