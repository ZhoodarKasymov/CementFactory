import time
from datetime import datetime
import pytz
import json
from dahua_rpc import DahuaRpc

# Test data simulating records returned from the Dahua device
test_data = {
    'params': {
        'records': [
            {'PlateNumber': 'ABC123', 'Time': int(time.time()) - 3600},  # 1 hour ago
            {'PlateNumber': 'XYZ789', 'Time': int(time.time()) - 7200},  # 2 hours ago
            {'PlateNumber': 'LMN456', 'Time': int(time.time()) - 10800},  # 3 hours ago
            {},  # Empty record to test fallback
            'Invalid data'  # Invalid entry to test the type check
        ]
    }
}

# Set the desired timezone (e.g., 'Asia/Dhaka')
timezone = pytz.timezone('Asia/Dhaka')

# Function to convert Unix timestamp to human-readable date
def unix_to_readable(unix_timestamp):
    return datetime.fromtimestamp(unix_timestamp, tz=timezone).strftime('%Y-%m-%d %H:%M:%S')

# Extract the records
records = test_data.get('params', {}).get('records', [])

output_data = []

# Process each record and store the Plate Number and time
for record in records:
    if isinstance(record, dict):  # Ensure each record is a dictionary
        plate_number = record.get('PlateNumber', 'N/A')  # Default to 'N/A' if missing
        timestamp = record.get('Time', 0)  # Default to 0 if missing
        readable_time = unix_to_readable(timestamp) if timestamp else 'N/A'
        
        # Append the result to output_data
        output_data.append({
            "PlateNumber": plate_number,
            "Time": readable_time
        })
    else:
        output_data.append({
            "PlateNumber": "N/A",
            "Time": "N/A",
            "Error": "Unexpected data format"
        })

# Return the data as a JSON string
print(json.dumps(output_data, indent=4))
