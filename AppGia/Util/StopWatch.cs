using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppGia.Util
{
    public class StopWatch
    {
        /**
	 * Identifier of this stop watch.
	 * Handy when we have output from multiple stop watches
	 * and need to distinguish between them in log or console output.
	 */
	private String id;

	private bool keepTaskList = true;

	private  LinkedList<TaskInfo> taskList = new LinkedList<TaskInfo>();

	/** Start time of the current task */
	private long startTimeMillis;

	/** Is the stop watch currently running? */
	private bool running;

	/** Name of the current task */
	private String currentTaskName;

	private TaskInfo lastTaskInfo;

	private int taskCount;

	/** Total running time */
	private long totalTimeMillis;


	/**
	 * Construct a new stop watch. Does not start any task.
	 */
	public StopWatch() {
		this.id = "";
	}

	/**
	 * Construct a new stop watch with the given id.
	 * Does not start any task.
	 * @param id identifier for this stop watch.
	 * Handy when we have output from multiple stop watches
	 * and need to distinguish between them.
	 */
	public StopWatch(String id) {
		this.id = id;
	}


	/**
	 * Determine whether the TaskInfo array is built over time. Set this to
	 * "false" when using a StopWatch for millions of intervals, or the task
	 * info structure will consume excessive memory. Default is "true".
	 */
	public void setKeepTaskList(bool keepTaskList) {
		this.keepTaskList = keepTaskList;
	}


	/**
	 * Start an unnamed task. The results are undefined if {@link #stop()}
	 * or timing methods are called without invoking this method.
	 * @see #stop()
	 */
	public void start() {
		start("");
	}

	/**
	 * Start a named task. The results are undefined if {@link #stop()}
	 * or timing methods are called without invoking this method.
	 * @param taskName the name of the task to start
	 * @see #stop()
	 */
	public void start(String taskName)  {
		if (this.running) {
			throw new InvalidOperationException("Can't start StopWatch: it's already running");
		}

		;
		this.startTimeMillis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		this.running = true;
		this.currentTaskName = taskName;
	}

	/**
	 * Stop the current task. The results are undefined if timing
	 * methods are called without invoking at least one pair
	 * {@link #start()} / {@link #stop()} methods.
	 * @see #start()
	 */
	public void stop()  {
		if (!this.running) {
			throw new InvalidOperationException("Can't stop StopWatch: it's not running");
		}
		long lastTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - this.startTimeMillis;
		this.totalTimeMillis += lastTime;
		this.lastTaskInfo = new TaskInfo(this.currentTaskName, lastTime);
		if (this.keepTaskList) {
			this.taskList.AddLast(lastTaskInfo);
		}
		++this.taskCount;
		this.running = false;
		this.currentTaskName = null;
	}

	/**
	 * Return whether the stop watch is currently running.
	 */
	public bool isRunning() {
		return this.running;
	}


	/**
	 * Return the time taken by the last task.
	 */
	public long getLastTaskTimeMillis()  {
		if (this.lastTaskInfo == null) {
			throw new InvalidOperationException("No tasks run: can't get last task interval");
		}
		return this.lastTaskInfo.getTimeMillis();
	}

	/**
	 * Return the name of the last task.
	 */
	public String getLastTaskName() {
		if (this.lastTaskInfo == null) {
			throw new InvalidOperationException("No tasks run: can't get last task name");
		}
		return this.lastTaskInfo.getTaskName();
	}

	/**
	 * Return the last task as a TaskInfo object.
	 */
	public TaskInfo getLastTaskInfo() {
		if (this.lastTaskInfo == null) {
			throw new InvalidOperationException("No tasks run: can't get last task info");
		}
		return this.lastTaskInfo;
	}


	/**
	 * Return the total time in milliseconds for all tasks.
	 */
	public long getTotalTimeMillis() {
		return this.totalTimeMillis;
	}

	/**
	 * Return the total time in seconds for all tasks.
	 */
	public double getTotalTimeSeconds() {
		return this.totalTimeMillis / 1000.0;
	}

	/**
	 * Return the number of tasks timed.
	 */
	public int getTaskCount() {
		return this.taskCount;
	}

	/**
	 * Return an array of the data for tasks performed.
	 */
	public TaskInfo[] getTaskInfo() {
		if (!this.keepTaskList) {
			throw new NotSupportedException("Task info is not being kept!");
		}
		return this.taskList.ToArray();
	}


	/**
	 * Return a short description of the total running time.
	 */
	public String shortSummary() {
		return "StopWatch '" + this.id + "': running time (millis) = " + getTotalTimeMillis();
	}

	/**
	 * Return a string with a table describing all tasks performed.
	 * For custom reporting, call getTaskInfo() and use the task info directly.
	 */
	public String prettyPrint() {
		StringBuilder sb = new StringBuilder(shortSummary());
		sb.Append('\n');
		if (!this.keepTaskList) {
			sb.Append("No task info kept");
		} else {
			sb.Append("-----------------------------------------\n");
			sb.Append("ms     %     Task name\n");
			sb.Append("-----------------------------------------\n");
			foreach (TaskInfo task in getTaskInfo()) {
				sb.Append(task.getTimeMillis()).Append("  ");
				sb.Append(formatPercentage(task.getTimeSeconds() / getTotalTimeSeconds())).Append("  ");
				sb.Append(task.getTaskName()).Append("\n");
			}
		}
		return sb.ToString();
	}

	private string formatPercentage(double val)
	{
		return "%"+(Math.Round(val, 3)*100);
	}

	/**
	 * Return an informative string describing all tasks performed
	 * For custom reporting, call {@code getTaskInfo()} and use the task info directly.
	 */
	public String toString() {
		StringBuilder sb = new StringBuilder(shortSummary());
		if (this.keepTaskList) {
			foreach (TaskInfo task in getTaskInfo()) {
				sb.Append("; [").Append(task.getTaskName()).Append("] took ").Append(task.getTimeMillis());
				double percent = Math.Round((100.0 * task.getTimeSeconds()) / getTotalTimeSeconds());
				sb.Append(" = ").Append(percent).Append("%");
			}
		} else {
			sb.Append("; no task info kept");
		}
		return sb.ToString();
	}


	/**
	 * Inner class to hold data about one task executed within the stop watch.
	 */
	public   class TaskInfo {

		private  String taskName;

		private  long timeMillis;

		public TaskInfo(String taskName, long timeMillis) {
			this.taskName = taskName;
			this.timeMillis = timeMillis;
		}

		/**
		 * Return the name of this task.
		 */
		public String getTaskName() {
			return this.taskName;
		}

		/**
		 * Return the time in milliseconds this task took.
		 */
		public long getTimeMillis() {
			return this.timeMillis;
		}

		/**
		 * Return the time in seconds this task took.
		 */
		public double getTimeSeconds() {
			return this.timeMillis / 1000.0;
		}
	}
    }
}