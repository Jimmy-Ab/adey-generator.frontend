
function DownloadPDF(fileName) {

    const cont = this.document.getElementById("cont");
    console.log(fileName);
    console.log(window);
    var opt = {
        margin: 1,
        filename: fileName,
        image: { type: 'jpeg', quality: 1 },
        html2canvas: { scale: 2 },
        jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' }
    };

    html2pdf().from(cont).set(opt).save();
}

