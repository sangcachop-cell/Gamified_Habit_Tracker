<script>
    function updateCountdown() {
        const cards = document.querySelectorAll(".card");

        cards.forEach(card => {
            const deadlineEl = card.querySelector(".deadline");
            const countdownEl = card.querySelector(".countdown");

            if (!deadlineEl || !deadlineEl.dataset.deadline) return;

            const deadline = new Date(deadlineEl.dataset.deadline);
            const now = new Date();

            const diff = deadline - now;

            if (diff <= 0) {
                countdownEl.innerHTML = "⛔ Expired";
                return;
            }

            const hours = Math.floor(diff / (1000 * 60 * 60));
            const minutes = Math.floor((diff / (1000 * 60)) % 60);

            countdownEl.innerHTML = `⏳ ${hours}h ${minutes}m left`;

            // 🔔 cảnh báo 10 phút
            if (diff <= 600000) {
                countdownEl.innerHTML += " 🔔 Hurry!";
            }
        });
    }

    setInterval(updateCountdown, 1000);
    updateCountdown();
</script>