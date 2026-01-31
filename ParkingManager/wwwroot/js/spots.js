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
              let errorText;
              try {
                const errorJson = await res.json();
                errorText = errorJson.error || JSON.stringify(errorJson);
              } catch {
                errorText = await res.text();
              }
              alert(`Booking failed: ${errorText}`);
              return;
            }

            alert(`Spot ${spot.spotNumber} booked successfully!`);
            location.reload();
          } catch (err) {
            alert(`Network error while booking: ${err}`);
          }
        };

        grid.appendChild(btn);
      });
    } catch (err) {
      alert(`Error loading spots: ${err}`);
      console.error("Error loading spots:", err);
    }
  }

  loadSpots();
});