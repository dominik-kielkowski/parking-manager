document.addEventListener("DOMContentLoaded", () => {
  const spotsUrl = "/parking/spots";
  const bookUrl = "/parking/book?spotNumber=";

  async function loadSpots() {
    try {
      const response = await fetch(spotsUrl);
      const spots = await response.json();

      const grid = document.getElementById("spotsGrid");
      grid.innerHTML = "";

      spots.forEach((spot) => {
        const btn = document.createElement("button");
        btn.textContent = spot.spotNumber;
        btn.className = "spotButton";

        switch (spot.spotType) {
          case 0: // Regular
            btn.classList.add("regularSpot");
            break;
          case 1: // Disabled
            btn.classList.add("disabledSpot");
            break;
          case 2: // Manager
            btn.classList.add("managerSpot");
            break;
        }

        if (spot.isTaken) {
          btn.classList.add("taken");
          btn.disabled = true;
        }

        btn.onclick = async () => {
          const res = await fetch(`${bookUrl}${spot.spotNumber}`, {
            method: "POST",
          });
          const text = await res.text();

          btn.classList.add("taken");
          btn.disabled = true;

          console.log(text);
        };

        grid.appendChild(btn);
      });
    } catch (err) {
      console.error("Error loading spots:", err);
    }
  }

  loadSpots();
});
