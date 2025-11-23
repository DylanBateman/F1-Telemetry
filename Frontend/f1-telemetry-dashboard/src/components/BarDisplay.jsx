const BarDisplay = ({ label, value }) => {
  const chartValue = value != null ? Math.max(0, Math.min(100, value)) : null

  return (
    <div className="space-y-2 text-xs text-gray-100">
      <div className="flex justify-between text-xs text-gray-400">
        <span>{label}</span>
        <span className="text-gray-300">
          {chartValue != null ? `${chartValue.toFixed(0)}%` : "—"}
        </span>
      </div>

      <div className="h-1.5 overflow-hidden rounded-full bg-white/10">
        <div
          className="h-full rounded-full bg-gradient-to-r from-[#cedc00] to-[#6df0c0] transition-all duration-300"
          style={{ width: chartValue != null ? `${chartValue}%` : "0%" }}
        />
      </div>
    </div>
  )
}

export default BarDisplay
