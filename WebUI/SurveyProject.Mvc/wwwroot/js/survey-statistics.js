function createMultipleChoicesChart(id){
    let serializedData = document.getElementById(`chart-data-${id}`).value;
    let deserializedData = JSON.parse(serializedData);
    let data = [];

    for (let i = 0; i < deserializedData.length; i++) {
        data.push({
            text: deserializedData[i].Text,
            count: deserializedData[i].Count
        })
    }

    new Chart(
        document.getElementById(`multiple-choices-chart-${id}`), {
            type: 'bar',
            data: {
                labels: data.map(row => row.text),
                datasets: [{
                    label: 'Choosen options',
                    data: data.map(row => row.count)
                }]
            }
        }
    );
}