import BarDisplay from "./BarDisplay"
import DisplayCard from "./DisplayCard"
import TyreCard from "./TyreCard"

export default function AllTelemetryDashboard({ telemetry, connectionStatus }) {
  if (!telemetry) {
    return (
      <div className="rounded-xl border border-white/10 bg-white/5 p-6 text-gray-200">
        <p className="text-base font-semibold">Waiting for telemetry…</p>
        <p className="text-sm text-gray-400">
          Live data will appear here as it is received.
        </p>
      </div>
    )
  }

  const tyres = [
    {
      position: "Front Left",
      temp: telemetry.FrontLeftTyreTempC,
      pressure: telemetry.FrontLeftTyrePressure,
    },
    {
      position: "Front Right",
      temp: telemetry.FrontRightTyreTempC,
      pressure: telemetry.FrontRightTyrePressure,
    },
    {
      position: "Rear Left",
      temp: telemetry.RearLeftTyreTempC,
      pressure: telemetry.RearLeftTyrePressure,
    },
    {
      position: "Rear Right",
      temp: telemetry.RearRightTyreTempC,
      pressure: telemetry.RearRightTyrePressure,
    },
  ]

  return (
    <section className="rounded-xl border border-white/10 bg-black/20 p-6 backdrop-blur">
      <div className="flex flex-col gap-2 md:flex-row md:items-end md:justify-between">
        <div className="flex flex-col gap-1">
          <div className="flex flex-wrap items-center gap-3">
            <h2 className="text-2xl font-bold text-gray-50">
              Car {telemetry.CarId}
            </h2>

            <span className="inline-flex items-center gap-2 rounded-full bg-[#cedc00]/10 px-3 py-1 text-xs font-semibold  text-[#cedc00]">
              {connectionStatus === "connected" ? (
                <span className="inline-block h-2 w-2 rounded-full bg-[#cedc00] animate-pulse" />
              ) : (
                <span className="inline-block h-2 w-2 rounded-full bg-red-400" />
              )}
              {connectionStatus === "connected"
                ? "Live - " +
                  new Date(telemetry.Timestamp).toLocaleTimeString([], {
                    hour: "2-digit",
                    minute: "2-digit",
                    second: "2-digit",
                  })
                : "Disconnected - Last data recieved: " +
                  new Date(telemetry.Timestamp).toLocaleTimeString([], {
                    hour: "2-digit",
                    minute: "2-digit",
                    second: "2-digit",
                  })}
            </span>
          </div>

          <span className="text-sm font-semibold text-[#cedc00]">
            Sector {telemetry.Sector}
          </span>
        </div>
      </div>

      <div className="mt-5 grid gap-5 lg:grid-cols-2 items-start">
        <div className="space-y-4">
          <div className="grid gap-3 md:grid-cols-3">
            <DisplayCard
              label="Speed"
              value={telemetry.SpeedKph?.toFixed(1) ?? "—"}
              unit="km/h"
            />
            <DisplayCard
              label="Engine Temp"
              value={
                telemetry.EngineTempC != null
                  ? telemetry.EngineTempC.toFixed(1)
                  : "—"
              }
              unit="°C"
            />
            <DisplayCard
              label="Fuel Level"
              value={
                telemetry.FuelLevel != null
                  ? telemetry.FuelLevel.toFixed(1)
                  : "—"
              }
              unit="%"
            />
          </div>

          <div className="grid gap-2">
            <BarDisplay label="Throttle" value={telemetry.Throttle} />
            <BarDisplay label="Brake" value={telemetry.Brake} />
          </div>
        </div>

        <div>
          <div className="grid grid-cols-2 gap-3">
            {tyres.map((t) => (
              <TyreCard
                key={t.position}
                position={t.position}
                temp={t.temp}
                pressure={t.pressure}
              />
            ))}
          </div>
        </div>
      </div>
    </section>
  )
}
