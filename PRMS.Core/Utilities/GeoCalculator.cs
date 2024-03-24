using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Utilities;

public static class GeoCalculator
{
    private const double EarthRadiusKm = 6371;

    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var dLat = DegreesToRadians(lat2 - lat1);
        var dLon = DegreesToRadians(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        var distance = EarthRadiusKm * c;
        return distance;
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}
