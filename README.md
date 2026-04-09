# Mileage Taxi Services
Simulating mileage-based taxi fares in Cities Skylines.

## Demystifying Taxis
I believe taxis are largely misunderstood, as seen from the comments in the page for Profitable Tourism, another mod that aims to increase taxi income.

It turns out, in Cities Skylines, taxi fares are collected ONLY when the journey is completed, which means taxis with passengers do not register any income DURING the journey.
The taxi fares are also very simple, only taking account into the straight-line distance (aka "displacement").

This mod aims to change that.

## Mod Status
- Requires After Dark DLC (duh)
- Requires Harmony
- Compatible with:
  - Ticket Price Customizer
  - Transport Lines Manager ("TLM")
  - Profitable Tourism

## Motivation and Technical Information
As mentioned above, the taxi fares in Cities Skylines are very simple, too simple indeed:

```
On arrival, calculate:
fare: DISPLACEMENT * 1/1000 * STANDARD_FARE_RATE
```

This does not take into account the actual mileage traveled. (In defense of CO: also tracking the actual mileage can be a performance problem!)

This also means taxis generally do not really "turn a profit".

This mod replaces the above fare with a mileage-based fare, at almost the same rate:

```
During journey with passenger, calculate:
mileage fare: DELTA_DISPLACEMENT * 1/1000 * STANDARD_FARE_RATE
idle fare: if mileage fare is 0, then collect fare as if DELTA_DISPLACEMENT is 1
At end-of-journey, collects base mileage fare equal to DELTA_DISPLACEMENT=500 (customizable by patching).
```

With this, the total fare is guaranteed to be higher than the vanilla fare, provided that the taxi is carrying passengers.
To further increase taxi income, consider using Ticket Price Customizer.

Taxis without passengers do NOT generate any income, just as usual.
