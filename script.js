// すべてのギャラリー画像を取得
const images = document.querySelectorAll(".slider-wrapper img");

//　モーダル要素の取得
const modal = document.getElementById("modal");
const modalImg = document.getElementById("modalImg");
const closeBtn = document.getElementById("closeBtn");

//　各種画像のクリックイベントを設定
images.forEach((image) => {
  image.addEventListener("click", () => {
    modal.classList.remove("hidden");
    modalImg.src = image.src;
  });
});

// 閉じるボタンをクリックした時
closeBtn.addEventListener("click", () => {
  modal.classList.add("hidden");
});

// モーダルの背景をクリックした時にモーダルを閉じる
modal.addEventListener("click", (event) => {
  if (event.target === modal) {
    modal.classList.add("hidden");
  }
});

const scrollBtn = document.getElementById("scrollToTop");

// スクロール時の表示切り替え
window.addEventListener("scroll", () => {
  if (window.scrollY > 300) {
    scrollBtn.classList.remove("hidden");
  } else {
    scrollBtn.classList.add("hidden");
  }
});

// ボタンクリックで上に戻る
scrollBtn.addEventListener("click", () => {
  window.scrollTo({
    top: 0,
    behavior: "smooth", // スムーズにスクロール
  });
});

const fadeElems = document.querySelectorAll(".fade-in-section");

const observer = new IntersectionObserver(
  (entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        entry.target.classList.add("visible");
        observer.unobserve(entry.target); // 一度表示したら監視を外す
      }
    });
  },
  {
    threshold: 0.1, // 10%表示されたら発火
  }
);

fadeElems.forEach((elem) => {
  observer.observe(elem);
});
