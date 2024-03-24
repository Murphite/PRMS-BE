using System.ComponentModel;

namespace PRMS.Domain.Enums
{
    public enum MedicationStatus
    {
        [Description("Pending")] Pending,
        [Description("Rejected")] Rejected,
        [Description("Approved")] Approved,
        [Description("Pending Review")] PendingReview,
        [Description("On Medication")] OnMedication,
        [Description("Finished")] Finished
    }
}
