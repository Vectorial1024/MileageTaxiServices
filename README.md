# Mileage Taxi Services
Simulating mileage-based taxi fares in Cities Skylines.

## Demystifying Taxis
I believe taxis are largely misunderstood, as seen from the comments in the page for Profitable Tourism, another mod that aims to increase taxi income.

It turns out, in Cities Skylines, taxi fares are collected ONLY when the journey is completed, which means taxis with passengers do not register any income DURING the journey.
The taxi fares are also very simple, only taking account into the straight-line distance (aka "displacement").

This mod aims to change that.

## Mod Status
- Requires Harmony
- Compatible with:
  - Ticket Price Customizer
  - Transport Lines Manager ("TLM")
  - Profitable Tourism

## Motivation and Technical Information
As mentioned above, the taxi fares in Cities Skylines are very simple, too simple indeed:

```
On arrival, calculate:
fare: DISPLACEMENT * 1/1000 * STANDARD_FARE
```

This does not take into account the actual mileage traveled. (In defense of CO: also tracking the actual mileage can be a performance problem!)

This also means taxis generally do not really "turn a profit".

This mod introduces mileage-based fare on top of the above fare:

```
During journey with passenger, also calculate:
mileage fare: DELTA_DISPLACEMENT * 1/2000 * STANDARD_FARE
idle fare: if mileage fare is 0, then collect fare as if DELTA_DISPLACEMENT is 1
```

With this, at least +50% taxi income is guaranteed, provided that the taxi is carrying passengers.

Taxis without passengers do NOT generate any income.
