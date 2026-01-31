document.addEventListener("DOMContentLoaded", () => {
  const spotsUrl = "/parking/spots";
  const bookUrl = "/parking/book?spotNumber=";

  const spotTypeToClass = {
    Regular: "regularSpot",
    Disabled: "disabledSpot",
    DisabledPermit: "disabledSpot",
    Manager: "managerSpot",
    ManagerAccess: "managerSpot"
  };

  async function loadSpots() {
    try {
      const response = await fetch(spotsUrl);
      if (!response.ok) {
        const text = await response.text();
        alert(`Failed to load spots: ${text}`);
        return;
      }

      const spots = await response.json();
      const grid = document.getElementById("spotsGrid");
      grid.innerHTML = "";

      spots.forEach((spot) => {
        const btn = document.createElement("button");
        btn.textContent = spot.spotNumber;
        btn.className = "spotButton";

        const cssClass = spotTypeToClass[spot.spotType] || "regularSpot";
        btn.classList.add(cssClass);

        if (spot.isTaken) {
          btn.classList.add("taken");
          btn.disabled = true;
        }

        btn.onclick = async () => {
          try {
            const res = await fetch(`${bookUrl}${spot.spotNumber}`, {
              method: "POST",
            });

            if (!res.ok) {
              const text = await res.text();
              alert(`Booking failed: ${text}`);
              return;
            }

            alert(`Spot ${spot.spotNumber} booked successfully!`);
            location.reload();
          } catch (err) {
            console.error("Network error:", err);
            alert("Network error while booking");
          }
        };

        grid.appendChild(btn);
      });
    } catch (err) {
      console.error("Error loading spots:", err);
      alert("Error loading parking spots");
    }
  }

  loadSpots();
});