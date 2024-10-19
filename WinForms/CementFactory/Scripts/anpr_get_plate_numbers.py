import sys
import json
import time
from dahua_rpc import DahuaRpc
from datetime import datetime
import pytz

# Get arguments from command line
host = sys.argv[1]
username = sys.argv[2]
password = sys.argv[3]
year = int(sys.argv[4])
month = int(sys.argv[5])
day = int(sys.argv[6])
hour = int(sys.argv[7])
minute = int(sys.argv[8])

# Initialize DahuaRpc with host, username, and password
dahua = DahuaRpc(host=host, username=username, password=password)

try:
    # Login to the Dahua device
    dahua.login()

    # Get object_id for ANPR Plate Numbers
    object_id = dahua.get_traffic_info()

    # Timezone setup (adjust based on your region)
    timezone = pytz.timezone('Asia/Dhaka')

    # Define the specific date and time with timezone
    specific_date = timezone.localize(datetime(year, month, day, hour, minute, 0))

    # Convert specific date to Unix timestamp
    start_date = int(specific_date.timestamp())+21600
    end_date = int(time.time())+21600  # Current time

    # Start searching for ANPR plate numbers between start_date and end_date
    dahua.start_find(object_id, start_date, end_date)

    # Fetch the results
    response = dahua.do_find(object_id=object_id)

    # Load the JSON response
    data = response if isinstance(response, dict) else json.loads(response)

    # Safely extract 'records' with fallback to an empty list
    records = data.get('params', {}).get('records', []) or []

    # Function to convert Unix timestamp to human-readable date
    def unix_to_readable(unix_timestamp):
        return datetime.fromtimestamp(unix_timestamp, tz=timezone).strftime('%Y-%m-%d %H:%M:%S')

    # List to store formatted output
    output_data = [{
                "PlateNumber": "N/A",
                "Time": "N/A",
                "Error": "Unexpected data format"
            }]

    # Process each record and store the Plate Number and time
    for record in records:
        if isinstance(record, dict):  # Ensure each record is a dictionary
            plate_number = record.get('PlateNumber', 'N/A')
            timestamp = record.get('Time', 0)
            readable_time = unix_to_readable(timestamp-21600) if timestamp else 'N/A'

            # Append the result to output_data
            output_data.append({
                "PlateNumber": plate_number,
                "Time": readable_time
            })
        else:
            # Handle unexpected data format
            output_data.append({
                "PlateNumber": "N/A",
                "Time": "N/A",
                "Error": "Unexpected data format"
            })

    # Return the data as a JSON string
    print(json.dumps(output_data, indent=4))

except Exception as e:
    print(json.dumps([{
                "PlateNumber": "N/A",
                "Time": "N/A",
                "Error": "Unexpected data format"
            }], indent=4))