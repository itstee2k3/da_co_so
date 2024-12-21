//// Set new default font family and font color to mimic Bootstrap's default styling
//Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
//Chart.defaults.global.defaultFontColor = '#292b2c';

//// Bar Chart Example
//var ctx = document.getElementById("myBarChart");
//var myLineChart = new Chart(ctx, {
//  type: 'bar',
//  data: {
//    labels: ["January", "February", "March", "April", "May", "June"],
//    datasets: [{
//      label: "Revenue",
//      backgroundColor: "rgba(2,117,216,1)",
//      borderColor: "rgba(2,117,216,1)",
//      data: [4215, 5312, 6251, 7841, 9821, 14984],
//    }],
//  },
//  options: {
//    scales: {
//      xAxes: [{
//        time: {
//          unit: 'month'
//        },
//        gridLines: {
//          display: false
//        },
//        ticks: {
//          maxTicksLimit: 6
//        }
//      }],
//      yAxes: [{
//        ticks: {
//          min: 0,
//          max: 15000,
//          maxTicksLimit: 5
//        },
//        gridLines: {
//          display: true
//        }
//      }],
//    },
//    legend: {
//      display: false
//    }
//  }
//});

// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

// Khởi tạo mảng doanh thu cho 6 tháng gần nhất
var revenueData = [0, 0, 0, 0, 0, 0];

// Tạo một đối tượng XMLHttpRequest
var xhr = new XMLHttpRequest();

// Thiết lập hàm xử lý cho sự kiện load
xhr.onload = function () {
    if (xhr.status >= 200 && xhr.status < 300) {
        // Nếu yêu cầu thành công
        var data = JSON.parse(xhr.responseText); // Chuyển đổi dữ liệu nhận được thành JSON
        // Cập nhật mảng doanh thu và cập nhật biểu đồ
        updateChart(data);
    } else {
        // Nếu có lỗi, hiển thị lỗi trong console
        console.error('Request failed with status:', xhr.status);
    }
};

// Thiết lập hàm xử lý cho sự kiện error
xhr.onerror = function () {
    console.error('Request failed');
};

// Mở một yêu cầu GET đến endpoint của controller
xhr.open('GET', '/admin/index?handler=RevenueData');

// Gửi yêu cầu
xhr.send();

// Hàm cập nhật biểu đồ với dữ liệu doanh thu mới
function updateChart(data) {
    var currentDate = new Date();
    var currentMonth = currentDate.getMonth() + 1; // Tháng hiện tại
    var currentYear = currentDate.getFullYear(); // Năm hiện tại
    var labels = []; // Mảng chứa tên các tháng
    var revenueData = []; // Mảng chứa doanh thu
    console.log(data)

    // Lặp qua 6 tháng gần đây
    for (var i = 5; i >= 0; i--) {
        var month = currentMonth - i;
        var year = currentYear; // Lấy năm hiện tại
        if (month <= 0) {
            month += 12; // Tháng năm trước
            year--; // Giảm năm đi 1 nếu tháng là tháng của năm trước
        }
        var dataForMonth = data.find(entry => entry.year === year && entry.month === month);
        console.log(dataForMonth)

        if (dataForMonth) {
            labels.push("Month " + month);
            revenueData.push(dataForMonth.total);
            //revenueData.push(data[month - 1]);
        } else {
            labels.push("Month " + month);
            revenueData.push(0);
        }
        console.log(revenueData)
    }

    var ctx = document.getElementById("myBarChart");
    var myLineChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: "Revenue",
                backgroundColor: "rgba(2,117,216,1)",
                borderColor: "rgba(2,117,216,1)",
                data: revenueData,
            }],
        },
        options: {
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        var value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
                        return value.toLocaleString('en-US') + 'đ';
                    }
                }
            },
            scales: {
                xAxes: [{
                    gridLines: {
                        display: false
                    }
                }],
                yAxes: [{
                    ticks: {
                        min: 0,
                        maxTicksLimit: 5,
                        callback: function (value, index, values) {
                            return value.toLocaleString('en-US') + 'đ';
                        }
                    },
                    gridLines: {
                        display: true
                    }
                }],
            },
            legend: {
                display: false
            }
        }
    });
}
