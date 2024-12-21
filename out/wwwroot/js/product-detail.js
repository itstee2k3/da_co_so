import * as THREE from 'three';

import * as tf from 'https://cdn.jsdelivr.net/npm/@tensorflow/tfjs@2.8.6/dist/tf.min.js';
import * as faceMesh from 'https://cdn.jsdelivr.net/npm/@mediapipe/face_mesh@0.4.1633559619/face_mesh.min.js';
import * as faceDetection from 'https://cdn.jsdelivr.net/npm/@mediapipe/face_detection@0.4.1646425229/face_detection.min.js';
//import * as faceMesh from 'https://cdn.jsdelivr.net/npm/@mediapipe/face_mesh/face_mesh.min.js';

//import { FaceMesh } from 'https://cdn.jsdelivr.net/npm/@mediapipe/face_mesh/face_mesh.min.js';
//import { FaceMesh } from '@mediapipe/face_mesh';
import { AmbientLight, Vector3 } from 'three';
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';

//let scene, camera, renderer, glassesModel;
//let videoFeed, canvas, videoTexture, videoMaterial;
let faceMeshModel;
let scene, camera, renderer; // Khai báo các biến toàn cục
let cameraViewerScene, cameraViewerCamera, cameraViewerRenderer, glassesModel;

const videoFeed = document.getElementById('video-feed');

const canvasModel = document.getElementById('threejs-canvas');

document.addEventListener("DOMContentLoaded", function () {

    function initialize3DViewer() {
        const viewerContainer = document.getElementById('viewer-container');
        const container = document.getElementById('viewer');
        const viewerRenderer = new THREE.WebGLRenderer({ antialias: true });
        viewerRenderer.setClearColor(0xffffff); // Nền trắng
        viewerRenderer.setPixelRatio(container.devicePixelRatio);
        viewerRenderer.setSize(container.offsetWidth, container.offsetHeight);
        container.appendChild(viewerRenderer.domElement);

        const viewerScene = new THREE.Scene();

        const viewerCamera = new THREE.PerspectiveCamera(45, container.offsetWidth / container.offsetHeight, 0.1, 1000);
        viewerCamera.position.set(4, 5, 11);

        const viewerControls = new OrbitControls(viewerCamera, viewerRenderer.domElement);
        viewerControls.enableDamping = true;
        viewerControls.target.set(0, 1, 0); // Tập trung camera vào trung tâm mô hình
        viewerControls.update();

        const ambientLight = new THREE.AmbientLight(0xffffff, 1.5); // Ánh sáng trắng đồng đều
        viewerScene.add(ambientLight);

        const loader = new GLTFLoader().setPath('/images/model3d/glasses/');
        loader.load('scene.gltf', (gltf) => {
            const model = gltf.scene;
            model.scale.set(0.5, 0.5, 0.5); // Điều chỉnh tỷ lệ (x, y, z)
            model.position.set(0, 0, 0); // Đặt mô hình ở trung tâm
            viewerScene.add(model);

            viewerRenderer.render(viewerScene, viewerCamera);

        }, undefined, (error) => {
            console.error('Error loading model:', error);
        });

        function animate() {
            requestAnimationFrame(animate);
            viewerControls.update();
            viewerRenderer.render(viewerScene, viewerCamera);
        }

        animate();

        document.getElementById('show-model3d').addEventListener('click', () => {
            viewerContainer.style.display = 'flex';
            viewerRenderer.setSize(viewerContainer.offsetWidth * 0.8, viewerContainer.offsetHeight * 0.8);
            // Cập nhật camera
            viewerCamera.aspect = viewerContainer.offsetWidth / viewerContainer.offsetHeight;
            viewerCamera.updateProjectionMatrix();
        });

        document.getElementById('close-viewer').addEventListener('click', () => {
            viewerContainer.style.display = 'none';
        });

        window.addEventListener('resize', () => {
            const width = viewerContainer.offsetWidth * 0.8;
            const height = viewerContainer.offsetHeight * 0.8;
            viewerRenderer.setSize(width, height);

            viewerCamera.aspect = width / height;
            viewerCamera.updateProjectionMatrix();
        });
    }





    // TRY MODEL 3D FUNCTIONALITY
    function initializeCameraViewer() {
        const closeBtn = document.getElementById('close-camera-viewer');

        cameraViewerScene = new THREE.Scene();
        cameraViewerCamera = new THREE.PerspectiveCamera(75, canvasModel.clientWidth / canvasModel.clientHeight, 0.1, 1000);
        cameraViewerRenderer = new THREE.WebGLRenderer({ canvas: canvasModel, alpha: true });
        cameraViewerRenderer.setSize(canvasModel.clientWidth, canvasModel.clientHeight);

        const light = new THREE.AmbientLight(0xffffff, 1);
        cameraViewerScene.add(light);

        const loader = new GLTFLoader().setPath('/images/model3d/glasses/');
        loader.load('scene.gltf', (gltf) => {
            console.log("Glasses model loaded:", gltf);
            glassesModel = gltf.scene;
            glassesModel.scale.set(0.5, 0.5, 0.5);
            glassesModel.position.set(0, 0, 0);
            cameraViewerScene.add(glassesModel);
            console.log("Glasses model loaded");
        }, undefined, (error) => {
            console.error('Error loading model:', error);
        });

        async function startCamera() {
            try {
                const stream = await navigator.mediaDevices.getUserMedia({ video: true });
                videoFeed.srcObject = stream;

                // Sửa ngược camera (selfie mode)
                videoFeed.style.transform = 'scaleX(-1)';
                videoFeed.style.webkitTransform = 'scaleX(-1)';

            ////    return new Promise(resolve => videoFeed.onloadedmetadata = resolve);
            } catch (error) {
                console.error('Error accessing camera:', error);
            }
        }

        document.getElementById('try-model3d').addEventListener('click', async () => {
            await startCamera();
            await initializeFaceMesh(cameraViewerRenderer, cameraViewerScene, cameraViewerCamera);
            document.getElementById('camera-viewer-container').style.display = 'flex';
        });

        document.getElementById('close-camera-viewer').addEventListener('click', () => {
            const tracks = videoFeed.srcObject.getTracks();
            tracks.forEach(track => track.stop());
            videoFeed.srcObject = null;
            document.getElementById('camera-viewer-container').style.display = 'none';
        });
    }

    function renderLoop() {
        if (cameraViewerRenderer && cameraViewerScene && cameraViewerCamera) {
            cameraViewerRenderer.render(cameraViewerScene, cameraViewerCamera);
        }
        requestAnimationFrame(renderLoop);
    }
    renderLoop();


    function updateGlassesPosition(landmarks) {
        const leftEye = landmarks[33];  // Left eye center
        const rightEye = landmarks[263]; // Right eye center

        if (leftEye && rightEye) {
            const canvasWidth = videoFeed.videoWidth;
            const canvasHeight = videoFeed.videoHeight;

            const leftEyePos = new THREE.Vector3(
                (leftEye.x - 0.5) * canvasWidth / canvasHeight, // X
                -(leftEye.y - 0.5), // Y (lật trục Y)
                -leftEye.z * 0.5 // Z
            );

            const rightEyePos = new THREE.Vector3(
                (rightEye.x - 0.5) * canvasWidth / canvasHeight,
                -(rightEye.y - 0.5),
                -rightEye.z * 0.5
            );

            const glassesPos = leftEyePos.clone().add(rightEyePos).multiplyScalar(0.5);
            glassesModel.position.copy(glassesPos);

            const angle = Math.atan2(
                rightEyePos.y - leftEyePos.y,
                rightEyePos.x - leftEyePos.x
            );
            glassesModel.rotation.set(0, 0, -angle);

            glassesModel.scale.set(0.5, 0.5, 0.5);
        }
    }


    // Initialize FaceDetection from MediaPipe
    async function initializeFaceMesh(renderer, scene, camera) {

        // Initialize the face detection model
        faceMeshModel = new FaceMesh({
            locateFile: (file) => `https://cdn.jsdelivr.net/npm/@mediapipe/face_mesh/${file}`,
            wasmSimd: false
        });

        faceMeshModel.initialize().then(() => {
            console.log('FaceMesh model initialized successfully.');
        }).catch((error) => {
            console.error('Error initializing FaceMesh:', error);
        });


        faceMeshModel.setOptions({
            locateLandmarks: true,
            maxNumFaces: 1,
            refineLandmarks: true, // Bật tinh chỉnh các điểm đặc trưng (giúp cải thiện độ chính xác)
            minDetectionConfidence: 0.5,
            minTrackingConfidence: 0.5
        });

        faceMeshModel.onResults((results) => {
            if (results.multiFaceLandmarks && results.multiFaceLandmarks.length > 0) {
                const landmarks = results.multiFaceLandmarks[0];
                updateGlassesPosition(landmarks);
            }
        });

        if (!faceMeshModel) {
            console.error('FaceMesh model is not loaded');
            return;
        }

        async function processVideoFrame() {
            if (videoFeed.readyState >= 2) {
                await faceMeshModel.send({ image: videoFeed });
            }
            requestAnimationFrame(processVideoFrame);
        }
        processVideoFrame();

        console.log('FaceMesh model loaded successfully');
    }

    // Handle results from face detection and update glasses position

    const videoFrame = async () => {
        await faceMeshModel.send({ image: videoFeed });
        requestAnimationFrame(videoFrame);
    };

    videoFeed.addEventListener('loadeddata', () => {
        videoFrame();
    });


    // Khởi tạo cả 2 viewer
    initialize3DViewer();
    initializeCameraViewer();




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