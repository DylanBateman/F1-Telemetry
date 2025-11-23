import { useEffect, useState } from "react"
import DisplayCard from "./DisplayCard"
import TyreCard from "./TyreCard"

const createEmptySectorStats = () => ({
  count: 0,
  speedSum: 0,
  engineTempSum: 0,
  tyres: {
    frontLeftTempSum: 0,
    frontRightTempSum: 0,
    rearLeftTempSum: 0,
    rearRightTempSum: 0,
    frontLeftPressureSum: 0,
    frontRightPressureSum: 0,
    rearLeftPressureSum: 0,
    rearRightPressureSum: 0,
  },
})

export default function SectorDashboard({ sector, telemetry, isActive }) {
  const [stats, setStats] = useState(() => createEmptySectorStats())

  useEffect(() => {
    if (!telemetry || telemetry.Sector !== sector) return

    setStats((prev) => {
      const prevTyres = prev.tyres

      return {
        count: prev.count + 1,
        speedSum: prev.speedSum + (telemetry.SpeedKph ?? 0),
        engineTempSum: prev.engineTempSum + (telemetry.EngineTempC ?? 0),
        tyres: {
          frontLeftTempSum:
            prevTyres.frontLeftTempSum + (telemetry.FrontLeftTyreTempC ?? 0),
          frontRightTempSum:
            prevTyres.frontRightTempSum + (telemetry.FrontRightTyreTempC ?? 0),
          rearLeftTempSum:
            prevTyres.rearLeftTempSum + (telemetry.RearLeftTyreTempC ?? 0),
          rearRightTempSum:
            prevTyres.rearRightTempSum + (telemetry.RearRightTyreTempC ?? 0),
          frontLeftPressureSum:
            prevTyres.frontLeftPressureSum + (telemetry.FrontLeftTyrePressure ?? 0),
          frontRightPressureSum:
            prevTyres.frontRightPressureSum + (telemetry.FrontRightTyrePressure ?? 0),
          rearLeftPressureSum:
            prevTyres.rearLeftPressureSum + (telemetry.RearLeftTyrePressure ?? 0),
          rearRightPressureSum:
            prevTyres.rearRightPressureSum + (telemetry.RearRightTyrePressure ?? 0),
        },
      }
    })
  }, [telemetry, sector])

  const count = stats.count
  const averageOrNull = (sum) => (count > 0 ? sum / count : null)

  const averageSpeed = averageOrNull(stats.speedSum)
  const averageEngineTemp = averageOrNull(stats.engineTempSum)

  const tyres = [
    {
      position: "Front Left Averages",
      temp: averageOrNull(stats.tyres.frontLeftTempSum),
      pressure: averageOrNull(stats.tyres.frontLeftPressureSum),
    },
    {
      position: "Front Right Averages",
      temp: averageOrNull(stats.tyres.frontRightTempSum),
      pressure: averageOrNull(stats.tyres.frontRightPressureSum),
    },
    {
      position: "Rear Left Averages",
      temp: averageOrNull(stats.tyres.rearLeftTempSum),
      pressure: averageOrNull(stats.tyres.rearLeftPressureSum),
    },
    {
      position: "Rear Right Averages",
      temp: averageOrNull(stats.tyres.rearRightTempSum),
      pressure: averageOrNull(stats.tyres.rearRightPressureSum),
    },
  ]

  const lastUpdated =
    telemetry?.Timestamp &&
    new Date(telemetry.Timestamp).toLocaleTimeString([], {
      hour: "2-digit",
      minute: "2-digit",
      second: "2-digit",
    })

  return (
    <div
      className={`rounded-md border border-white/10 bg-black/30 p-4 text-gray-100 backdrop-blur-sm transition-colors ${
        isActive ? "ring-1 ring-[#cedc00]/70 border-[#cedc00]/60" : ""
      }`}
    >
      <div className="flex items-center justify-between mb-3">
        <div className="flex items-center gap-2">
          <div className="text-sm font-semibold">Sector {sector}</div>
          <span
            className={`inline-flex items-center gap-1 rounded-full px-2 py-1 text-[10px] font-semibold uppercase tracking-wide ${
              isActive
                ? "bg-[#cedc00]/20 text-[#cedc00]"
                : "bg-white/5 text-gray-300"
            }`}
          >
            <span
              className={`h-2 w-2 rounded-full ${
                isActive ? "bg-[#cedc00] animate-pulse" : "bg-gray-400"
              }`}
            />
            {isActive ? "Active" : "Waiting"}
          </span>
        </div>
        <div className="text-[11px] text-gray-400">
          {lastUpdated ? `Last update ${lastUpdated}` : "Waiting for data…"}
        </div>
      </div>

      <div className="grid grid-cols-2 gap-2">
        <DisplayCard
          label="Avg Speed"
          value={averageSpeed != null ? averageSpeed.toFixed(1) : "—"}
          unit="km/h"
        />
        <DisplayCard
          label="Avg Engine Temp"
          value={
            averageEngineTemp != null ? averageEngineTemp.toFixed(1) : "—"
          }
          unit="°C"
        />
      </div>

      <div className="grid grid-cols-2 gap-2 mt-3">
        {tyres.map((t) => (
          <TyreCard key={t.position} {...t} />
        ))}
      </div>
    </div>
  )
}
