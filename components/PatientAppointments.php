  <script>
  $(function() {
    $( "#datepicker1" ).datepicker();
    $( "#datepicker2" ).datepicker();
  });
  </script>

<div class="container">
  <div class="well"  style="background-color: white;">
    <fieldset>
      <legend>
        <h3>
          <abbr title="Given Name">
            Ali
          </abbr>
          &nbsp;
          <abbr title="Family Name">
            Hassan
          </abbr>
          <small>
            &nbsp;Female 14 year(s) (~17.Feb.2002)
          </small>

          <button type="button" class="btn btn-link">Edit</button>
          <button type="button" class="btn btn-link" data-toggle="collapse" data-target="#demo">Hide Contact Info <span id="" class="glyphicon glyphicon-chevron-down"></span></button>

        </h3>
    </legend>
<div id="demo" class="collapse">

        <h4>
        <small>
          <abbr title="Address">
            123, dream street, cPulao44330
          </abbr>
          <span>&nbsp;</span>
          <abbr title="Telephone Number">
            804209211
          </abbr>
          <button type="button" class="btn btn-link">Edit</button>
        </small>
        </h4>
</div>
<br>
</fieldset>

<div class="well">
  <div class="row">
    <div class="col-md-8">
      
      <div class="panel panel-primary">
        <div class="panel-heading">
          <span class="glyphicon glyphicon-calender" style="float: left"></span>
          &nbsp;&nbsp;Patient Appointments
          <span class="glyphicon glyphicon-calendar" style="float: right"></span>
        </div>
        <div class="panel-body">

              <form role="form">
      <div class="form-group">
        <div class="row">
          <div class="col-md-4">
            <label for="email">Timeframe (Optional)</label>
            <input type="email" class="form-control" id="datepicker1">
          </div>
          <div class="col-md-4">
            <label for="email">&nbsp;</label>
            <input type="email" class="form-control" id="datepicker2">
          </div>
        </div>
      </div>
    </form>  



        </div>
      </div>

      <div class="panel panel-primary">
        <div class="panel-heading">
          <span class="glyphicon glyphicon-calendar" style="float: left"></span>
          &nbsp;&nbsp;Appointment Requests
          <span class="glyphicon glyphicon-pencil" style="float: right"></span>
        </div>
        <div class="panel-body">
          <?php include("PatientAppointments_PatientTable.php"); ?>
        </div>
      </div>

      <div class="panel panel-primary">
        <div class="panel-heading">
          <span class="glyphicon glyphicon-fire" style="float: left"></span>
          &nbsp;&nbsp;Schedule A New Appointment
          <span class="glyphicon glyphicon-pencil" style="float: right"></span>
        </div>
        <div class="panel-body">
                  <div class="panel-body">

    
              <form role="form">
      <div class="form-group">
        <div class="row">
          <div class="col-md-4">
            <label for="email">Select Service Type</label>
            <input type="email" class="form-control" >
          </div>

          <div class="col-md-4">
            <label for="email">Timeframe (Optional)</label>
            <input type="email" class="form-control" id="datepicker1">
          </div>
          <div class="col-md-4">
            <label for="email">&nbsp;</label>
            <input type="email" class="form-control" id="datepicker2">
          </div>
        </div>

        <div class="row">
          <div class="col-md-4">
            <label for="email">Show Full Time Blocks</label>
            <input type="checkbox" value="">
          </div>
        </div>

        <button type="button" class="btn btn-danger">Back</button>
        <button type="button" class="btn btn-primary">Search</button>
      </div>


    </form>  



        </div>
      </div>
    </div>
  </div>
</div>
</div>