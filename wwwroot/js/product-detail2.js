import * as THREE from 'three';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js';

const webcamElement = document.getElementById('webcam');
const canvasElement = document.getElementById('canvas');
const webcam = new Webcam(webcamElement, 'user');
let selectedglasses = $(".selected-glasses img");
let isVideo = false;
let model = null;
let cameraFrame = null;
let detectFace = false;
let clearglasses = false;
let glassesOnImage = false;
let glassesArray = [];
let scene;
let camera;
let renderer;
let obControls;
let glassesKeyPoints = { midEye: 168, leftEye: 143, noseBottom: 2, rightEye: 372 };

document.addEventListener("DOMContentLoaded", function () {

    const viewerContainer = document.getElementById('viewer-container');

    document.getElementById('show-model3d').addEventListener('click', () => {
        viewerContainer.style.display = 'flex';
        // Lấy kích thước của viewer container
        const containerWidth = viewerContainer.offsetWidth * 0.8;
        const containerHeight = viewerContainer.offsetHeight * 0.8;

        // Đặt lại kích thước renderer và camera
        renderer.setSize(containerWidth, containerHeight);
        camera.aspect = containerWidth / containerHeight;
        camera.updateProjectionMatrix();
        //canvasElement.setSize(viewerContainer.offsetWidth * 0.8, viewerContainer.offsetHeight * 0.8);
        // Cập nhật camera
        //viewerCamera.aspect = viewerContainer.offsetWidth / viewerContainer.offsetHeight;
        //viewerCamera.updateProjectionMatrix();
    });

    window.addEventListener('resize', () => {
        // Lấy kích thước của container
        const containerWidth = viewerContainer.offsetWidth * 0.8;
        const containerHeight = viewerContainer.offsetHeight * 0.8;

        // Cập nhật kích thước renderer
        renderer.setSize(containerWidth, containerHeight);

        // Cập nhật camera
        camera.aspect = containerWidth / containerHeight;
        camera.updateProjectionMatrix();
    });


    document.getElementById('close-viewer').addEventListener('click', () => {
        viewerContainer.style.display = 'none';
    });

    $(document).ready(function () {
        setup3dScene();
        setup3dCamera();
        setup3dGlasses();
    });

    $("#webcam-switch").change(function () {
        if (this.checked) {
            $('.md-modal').addClass('md-show');
            webcam.start()
                .then(result => {
                    console.log("webcam started");
                    isVideo = true;
                    cameraStarted();
                    switchSource();
                    glassesOnImage = false;
                    startVTGlasses();
                })
                .catch(err => {
                    displayError();
                });
        }
        else {
            webcam.stop();
            if (cameraFrame != null) {
                clearglasses = true;
                detectFace = false;
                cancelAnimationFrame(cameraFrame);
            }
            isVideo = false;
            switchSource();
            cameraStopped();
            console.log("webcam stopped");
        }
    });


    $('#closeError').click(function () {
        $("#webcam-switch").prop('checked', false).change();
    });

    document.getElementById('try-model3d').addEventListener('click', () => {
        $('.md-modal').addClass('md-show');
        webcam.start()
            .then(result => {
                console.log("webcam started");
                isVideo = true;
                cameraStarted();
                switchSource();
                glassesOnImage = false;
                startVTGlasses();
            })
            .catch(err => {
                displayError();
            });
    });

    document.getElementById('close-camera-viewer').addEventListener('click', () => {
        webcam.stop();
        if (cameraFrame != null) {
            clearglasses = true;
            detectFace = false;
            cancelAnimationFrame(cameraFrame);
        }
        isVideo = false;
        switchSource();
        cameraStopped();
        console.log("webcam stopped");
    });

    async function startVTGlasses() {
        return new Promise((resolve, reject) => {
            $(".loading").removeClass('d-none');
            faceLandmarksDetection.load(faceLandmarksDetection.SupportedPackages.mediapipeFacemesh).then(mdl => {
                model = mdl;
                console.log("model loaded");
                if (isVideo && webcam.facingMode == 'user') {
                    detectFace = true;
                }

                cameraFrame = detectFaces().then(() => {
                    $(".loading").addClass('d-none');
                    resolve();
                });
            })
                .catch(err => {
                    displayError('Fail to load face mesh model<br/>Please refresh the page to try again');
                    reject(error);
                });
        });
    }

    async function detectFaces() {
        let inputElement = webcamElement;
        let flipHorizontal = !isVideo;

        await model.estimateFaces
            ({
                input: inputElement,
                returnTensors: false,
                flipHorizontal: flipHorizontal,
                predictIrises: false
            }).then(faces => {
                //console.log(faces);
                drawglasses(faces).then(() => {
                    if (clearglasses) {
                        clearCanvas();
                        clearglasses = false;
                    }
                    if (detectFace) {
                        cameraFrame = requestAnimFrame(detectFaces);
                    }
                });
            });
    }

    async function drawglasses(faces) {
        if (isVideo && (glassesArray.length != faces.length)) {
            clearCanvas();
            for (let j = 0; j < faces.length; j++) {
                await setup3dGlasses();
            }
        }

        for (let i = 0; i < faces.length; i++) {
            let glasses = glassesArray[i];
            let face = faces[i];
            if (typeof glasses !== "undefined" && typeof face !== "undefined") {
                let pointMidEye = face.scaledMesh[glassesKeyPoints.midEye];
                let pointleftEye = face.scaledMesh[glassesKeyPoints.leftEye];
                let pointNoseBottom = face.scaledMesh[glassesKeyPoints.noseBottom];
                let pointrightEye = face.scaledMesh[glassesKeyPoints.rightEye];

                // Kiểm tra xem có đủ 2 mắt không
                let isOnlyOneEyeVisible = !pointleftEye || !pointrightEye;

                if (isOnlyOneEyeVisible) {
                    // Nếu chỉ có một mắt, sử dụng điểm mắt còn lại để giả định vị trí mắt còn lại
                    if (pointleftEye) {
                        pointrightEye = [pointleftEye[0] + 0.15, pointleftEye[1], pointleftEye[2]];
                    } else if (pointrightEye) {
                        pointleftEye = [pointrightEye[0] - 0.15, pointrightEye[1], pointrightEye[2]];
                    }
                }

                // Cập nhật vị trí kính sao cho kính nằm trên tai
                glasses.position.x = (pointMidEye[0] + pointleftEye[0] + pointrightEye[0]) / 3;
                glasses.position.y = -pointMidEye[1] + parseFloat(selectedglasses.attr("data-3d-up"));
                glasses.position.z = -camera.position.z + pointMidEye[2] - 0.1; // Điều chỉnh chút ít để kính không đâm vào mắt

                // Tính toán góc quay của kính sao cho kính luôn xoay sát với khuôn mặt
                let direction = new THREE.Vector3();
                direction.subVectors(new THREE.Vector3(pointleftEye[0], pointleftEye[1], pointleftEye[2]), new THREE.Vector3(pointrightEye[0], pointrightEye[1], pointrightEye[2]));
                let angle = Math.atan2(direction.y, direction.x);
                glasses.rotation.z = angle;  // Quay kính theo hướng của mắt

                // Tính toán vector hướng để chỉnh kính sao cho kính thẳng đứng
                let upDirection = new THREE.Vector3();
                upDirection.subVectors(new THREE.Vector3(pointMidEye[0], pointMidEye[1], pointMidEye[2]), new THREE.Vector3(pointNoseBottom[0], pointNoseBottom[1], pointNoseBottom[2]));
                upDirection.normalize();

                // Cập nhật tỷ lệ kính dựa trên khoảng cách giữa hai mắt
                const eyeDist = Math.sqrt(
                    (pointleftEye[0] - pointrightEye[0]) ** 2 +
                    (pointleftEye[1] - pointrightEye[1]) ** 2 +
                    (pointleftEye[2] - pointrightEye[2]) ** 2
                );
                glasses.scale.set(eyeDist * parseFloat(selectedglasses.attr("data-3d-scale")), eyeDist * parseFloat(selectedglasses.attr("data-3d-scale")), eyeDist * parseFloat(selectedglasses.attr("data-3d-scale")));

                // Sử dụng Quaternion để quay kính thay vì Euler, tránh gimbal lock
                let rotationQuat = new THREE.Quaternion();
                rotationQuat.setFromEuler(new THREE.Euler(0, Math.PI, Math.PI / 2 - Math.acos(upDirection.x)));
                glasses.setRotationFromQuaternion(rotationQuat);

                renderer.render(scene, camera);
            }
        }
    }

    function clearCanvas() {
        for (var i = scene.children.length - 1; i >= 0; i--) {
            var obj = scene.children[i];
            if (obj.type == 'Group') {
                scene.remove(obj);
            }
        }
        renderer.render(scene, camera);
        glassesArray = [];
    }

    function switchSource() {
        clearCanvas();
        let containerElement
        if (isVideo) {
            containerElement = $("#webcam-container");
        } else {
            containerElement = $("#image-container");
            setup3dGlasses();
        }
        setup3dCamera();
        $("#canvas").appendTo(containerElement);
        $(".loading").appendTo(containerElement);
    //    $("#glasses-slider").appendTo(containerElement);
    }

    function setup3dScene() {
        scene = new THREE.Scene();
        renderer = new THREE.WebGLRenderer({
            canvas: canvasElement,
            alpha: true
        });
        //renderer.setSize(window.innerWidth, window.innerHeight);
        //renderer.setPixelRatio(window.devicePixelRatio);
        //light
        var frontLight = new THREE.SpotLight(0xffffff, 0.3);
        frontLight.position.set(10, 10, 10);
        scene.add(frontLight);
        var backLight = new THREE.SpotLight(0xffffff, 0.3);
        backLight.position.set(10, 10, -10)
        scene.add(backLight);
    }


    function setup3dCamera() {
        if (isVideo) {
            camera = new THREE.PerspectiveCamera(45, 1, 0.1, 2000);
            let videoWidth = webcamElement.width;
            let videoHeight = webcamElement.height;
            camera.position.x = videoWidth / 2;
            camera.position.y = -videoHeight / 2;
            camera.position.z = -(videoHeight / 2) / Math.tan(45 / 2);
            camera.lookAt({ x: videoWidth / 2, y: -videoHeight / 2, z: 0, isVector3: true });
            renderer.setSize(videoWidth, videoHeight);
            renderer.setClearColor(0x000000, 0);
        }
        else {
            camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
            camera.position.set(0, 0, 1.5);
            camera.lookAt(scene.position);
            renderer.setSize(window.innerWidth, window.innerHeight);
            renderer.setClearColor(0x3399cc, 1);
            obControls = new OrbitControls(camera, renderer.domElement);
        }
        let cameraExists = false;
        scene.children.forEach(function (child) {
            if (child.type == 'PerspectiveCamera') {
                cameraExists = true;
            }
        });
        if (!cameraExists) {
            camera.add(new THREE.PointLight(0xffffff, 0.8));
            scene.add(camera);
        }
        setup3dAnimate();
    }

    async function setup3dGlasses() {
        return new Promise(resolve => {
            var threeType = selectedglasses.attr("data-3d-type");
            if (threeType == 'gltf') {
                var gltfLoader = new GLTFLoader();
                gltfLoader.setPath(selectedglasses.attr("data-3d-model-path"));
                gltfLoader.load(selectedglasses.attr("data-3d-model"), function (object) {
                    object.scene.position.set(selectedglasses.attr("data-3d-x"), selectedglasses.attr("data-3d-y"), selectedglasses.attr("data-3d-z"));
                    var scale = selectedglasses.attr("data-3d-scale");
                    if (window.innerWidth < 480) {
                        scale = scale * 0.5;
                    }
                    object.scene.scale.set(scale, scale, scale);
                    scene.add(object.scene);
                    glassesArray.push(object.scene);
                    resolve('loaded');
                });
            }
        });
    }

    var setup3dAnimate = function () {
        if (!isVideo) {
            requestAnimationFrame(setup3dAnimate);
            obControls.update();
        }
        renderer.render(scene, camera);
    };






    // Xử lý thêm sản phẩm vào giỏ hàng
    // Lấy các phần tử cần xử lý
    var displayNumber = document.getElementById('display-number');
    var buttonSubtract = document.getElementById('button-subtract');
    var buttonAdd = document.getElementById('button-add');
    var addToCartButton = document.getElementById('add-to-cart-button');

    // Khởi tạo giá trị ban đầu
    var quantity = 1;
    displayNumber.textContent = quantity;

    // Xử lý khi click vào nút trừ
    buttonSubtract.addEventListener('click', function () {
        // Kiểm tra nếu giá trị số hiện tại lớn hơn 1
        if (quantity > 1) {
            quantity -= 1;
            displayNumber.textContent = quantity;
        }
    });

    // Xử lý khi click vào nút cộng
    buttonAdd.addEventListener('click', function () {
        quantity += 1;
        displayNumber.textContent = quantity;
    });

    addToCartButton.addEventListener("click", function () {
        // Lấy ID sản phẩm từ thuộc tính data-product-id của thẻ button
        var productId = addToCartButton.getAttribute('data-product-id');

        // Lấy giá trị quantity từ nội dung của thẻ span
        var quantity = parseInt(displayNumber.textContent);

        // Kiểm tra nếu số lượng đặt hàng lớn hơn số lượng có sẵn
        if (quantity > productNums) {
            alert("The quantity you are trying to order exceeds the available stock. Remaining stock: " + productNums)
            return;
        }

        var xhr = new XMLHttpRequest();
        xhr.open("POST", '/Cart/AddToCart', true);
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    var response = JSON.parse(xhr.responseText);
                    if (response.error) {
                        alert("Log in to add to cart");
                        console.error("Đăng nhập để thêm vào giỏ hàng");
                    } else {
                        alert("Add to cart successfully!");
                        console.log("Thêm vào giỏ hàng thành công!");
                    }
                } else {
                    alert("Error sending request");
                    console.error("Lỗi khi gửi yêu cầu: " + xhr.statusText);
                }
            }
        };
        xhr.send('productId=' + productId + '&quantity=' + quantity);
    });

    var sizes = productSize;


    // Tách chuỗi thành mảng
    var sizeArray = sizes.split('-');

    // Hàm để kiểm tra và trả về giá trị hoặc 'N/A'
    function getSizeValue(size) {
        return size && size.trim() !== '' ? size : 'N/A';
    }

    // Gán giá trị cho các thẻ span
    document.getElementById('size1').innerText = getSizeValue(sizeArray[0]);
    document.getElementById('size2').innerText = getSizeValue(sizeArray[1]);
    document.getElementById('size3').innerText = getSizeValue(sizeArray[2]);

});